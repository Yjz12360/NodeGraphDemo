
local nCurrGameId = 0
local function genGameId()
    nCurrGameId = nCurrGameId + 1
    return nCurrGameId
end

local tSvrGames = {}

local function addAIUpdateTimer(tSvrGame)
    local nLastTime = TimeMod.getTime()
    tSvrGame.nAIUpdateTimer = TimerMod.add(0.2, function()
        local nCurrTime = TimeMod.getTime()
        local nDeltaTime = nCurrTime - nLastTime
        AIManagerMod.updateAI(tSvrGame.tAIManager, nDeltaTime)
        nLastTime = nCurrTime
    end)
end

function addGame(nGameConfigId)
    local tGameConfig = Config.Game[nGameConfigId]
    if tGameConfig == nil then
        printError("GameConfig not exist configId: " .. nGameConfigId)
        return
    end
    local sSceneConfig = tGameConfig.sSceneConfig
    local tGameSceneConfig = GameSceneCfgMod.getConfigByName(sSceneConfig)
    if tGameSceneConfig == nil then
        printError("GameSceneConfig not exist config: " .. sSceneConfig)
        return
    end

    local nGameId = genGameId()
    local tSvrGame = {}
    tSvrGame.nGameId = nGameId
    tSvrGame.tGameConfig = tGameConfig
    tSvrGame.tGameSceneConfig = tGameSceneConfig
    tSvrGame.tGameObjects = {}
    tSvrGame.tMainNodeGraph = nil
    tSvrGame.nAIUpdateTimer = 0
    tSvrGame.tAIManager = AIManagerMod.addAIManager(tSvrGame)
    addAIUpdateTimer(tSvrGame)
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
    SvrGameRoleMod.addPlayer(tSvrGame, 1, 0, 0, 0)
end

function delGame(nGameId)
    local tSvrGame = tSvrGames[nGameId]
    if tSvrGame == nil then
        return
    end
    local nAIUpdateTimer = tSvrGame.nAIUpdateTimer
    if nAIUpdateTimer ~= nil then
        TimerMod.remove(nAIUpdateTimer)
    end
    tSvrGames[nGameId] = nil
end

function startNodeGraph(tSvrGame, nConfigId)
    if tSvrGame == nil then
        return
    end
    local nNodeGraphId = 1
    local tNodeGraph = SvrNodeGraphMod.addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    tSvrGame.tMainNodeGraph = tNodeGraph
    tSvrGame.nCurrGameObjectId = 1
    Messager.S2CAddNodeGraph(tSvrGame.nGameId, nNodeGraphId, nConfigId)
    SvrNodeGraphMod.startNodeGraph(tNodeGraph)
end

function processEvent(tSvrGame, nEventType, ...)
    if tSvrGame == nil then
        return
    end
    local tMainNodeGraph = tSvrGame.tMainNodeGraph
    if tMainNodeGraph ~= nil then
        SvrNodeGraphMod.processEvent(tMainNodeGraph, nEventType, ...)
    end
end

function getNextObjectId(tSvrGame)
    local nObjectId = tSvrGame.nCurrGameObjectId
    tSvrGame.nCurrGameObjectId = nObjectId + 1
    return nObjectId
end

function getObjects(tSvrGame)
    if tSvrGame == nil then
        return
    end
    return tSvrGame.tGameObjects
end

function getObject(tSvrGame, nObjectId)
    if tSvrGame == nil then
        return
    end
    return tSvrGame.tGameObjects[nObjectId]
end
