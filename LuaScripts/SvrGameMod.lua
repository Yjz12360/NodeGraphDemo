
local nCurrGameId = 0
local function genGameId()
    nCurrGameId = nCurrGameId + 1
    return nCurrGameId
end

local tSvrGames = {}

function addGame(nGameConfigId)
    local tGameConfig = Config.Game[nGameConfigId]
    if tGameConfig == nil then
        printError("GameConfig not exist configId: " .. nGameConfigId)
        return
    end

    local nGameId = genGameId()
    local tSvrGame = {}
    tSvrGame.nGameId = nGameId
    tSvrGame.tGameConfig = tGameConfig
    tSvrGame.tGameObjects = {}
    tSvrGame.nCurrGameObjectId = 1
    tSvrGame.tMainNodeGraph = nil
    tSvrGame.nCurrNodeGraphId = 1
    tSvrGames[nGameId] = tSvrGame

    Messager.S2CCreateGameSucc(tSvrGame.nGameId)
    SvrGameMod.initGame(tSvrGame)
end

function getGameById(nGameId)
    return tSvrGames[nGameId]
end

function initGame(tSvrGame)
    local tGameConfig = tSvrGame.tGameConfig
    local nInitNodeGraph = tGameConfig.nInitNodeGraph
    if nInitNodeGraph ~= nil and nInitNodeGraph > 0 then
        SvrGameMod.startNodeGraph(tSvrGame, nInitNodeGraph)
    end
    SvrGameMod.addPlayer(tSvrGame, 1, 0, 0, 0)
end

function startNodeGraph(tSvrGame, nConfigId)
    if tSvrGame == nil then
        return
    end
    local nNodeGraphId = tSvrGame.nCurrNodeGraphId
    local tNodeGraph = SvrNodeGraphMod.addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    tSvrGame.tMainNodeGraph = tNodeGraph
    Messager.S2CAddNodeGraph(tSvrGame.nGameId, nNodeGraphId, nConfigId)
    tSvrGame.nCurrNodeGraphId = nNodeGraphId + 1
    SvrNodeGraphMod.startNodeGraph(tNodeGraph)
end

local function getNextObjectId(tSvrGame)
    local nObjectId = tSvrGame.nCurrGameObjectId
    tSvrGame.nCurrGameObjectId = nObjectId + 1
    return nObjectId
end

function addPlayer(tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    local tConfig = Config.Player[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local nObjectId = getNextObjectId(tSvrGame)
    local tPlayer = {}
    tPlayer.nObjectId = nObjectId
    tPlayer.nObjectType = Const.GameObjectType.Player
    tPlayer.tConfig = tConfig
    tPlayer.tPos = {
        x = nPosX,
        y = nPosY,
        z = nPosZ,
    }
    Messager.S2CAddPlayer(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
end

function onEnterTrigger(nGameId, nTriggerId)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then return end
    local tMainNodeGraph = tSvrGame.tMainNodeGraph
    if tMainNodeGraph ~= nil then
        SvrNodeGraphMod.handleEnterTrigger(tMainNodeGraph, nTriggerId)
    end
end

function update(nDeltaTime)
    for _, tSvrGame in pairs(tSvrGames) do
        SvrGameMod.updateGame(tSvrGame, nDeltaTime)
    end
end

function updateGame(tSvrGame, nDeltaTime)
    local tMainNodeGraph = tSvrGame.tMainNodeGraph
    if tMainNodeGraph ~= nil then
        SvrNodeGraphMod.updateNodeGraph(tMainNodeGraph, nDeltaTime)
        if SvrNodeGraphMod.isFinish(tMainNodeGraph) then
            tSvrGame.tMainNodeGraph = nil
        end
    end
end
