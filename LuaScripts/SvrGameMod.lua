
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
    tSvrGame.tMainNodeGraph = nil
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
    local nNodeGraphId = 1
    local tNodeGraph = SvrNodeGraphMod.addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    tSvrGame.tMainNodeGraph = tNodeGraph
    tSvrGame.nCurrGameObjectId = 1
    Messager.S2CAddNodeGraph(tSvrGame.nGameId, nNodeGraphId, nConfigId)
    SvrNodeGraphMod.startNodeGraph(tNodeGraph)
end

local function procNodeGraphEvent(tSvrGame, nEventType, ...)
    if tSvrGame == nil then
        return
    end
    local tMainNodeGraph = tSvrGame.tMainNodeGraph
    if tMainNodeGraph ~= nil then
        SvrNodeGraphMod.processEvent(tMainNodeGraph, nEventType, ...)
    end
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
    tPlayer.nCurrHP = tConfig.nHP
    tSvrGame.tGameObjects[nObjectId] = tPlayer
    Messager.S2CAddPlayer(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
end

function addMonster(tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    local tConfig = Config.Monster[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local nObjectId = getNextObjectId(tSvrGame)
    local tMonster = {}
    tMonster.nObjectId = nObjectId
    tMonster.nObjectType = Const.GameObjectType.Monster
    tMonster.tConfig = tConfig
    tMonster.tPos = {
        x = nPosX,
        y = nPosY,
        z = nPosZ,
    }
    tMonster.nCurrHP = tConfig.nHP
    tSvrGame.tGameObjects[nObjectId] = tMonster
    Messager.S2CAddMonster(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
    procNodeGraphEvent(tSvrGame, Const.EventType.AddMonster)
end

function roleDead(tSvrGame, nObjectId)
    local tRole = tSvrGame.tGameObjects[nObjectId]
    if tRole == nil then
        return
    end
    local tGameObject = tSvrGame.tGameObjects[nObjectId]
    tSvrGame.tGameObjects[nObjectId] = nil
    Messager.S2CRoleDead(tSvrGame.nGameId, nObjectId)
    
    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        procNodeGraphEvent(tSvrGame, Const.EventType.MonsterDead, nObjectId)
    end
end

function hasMonster(tSvrGame)
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            return true
        end
    end
    return false
end

function getMonsterNum(tSvrGame)
    local nCount = 0
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            nCount = nCount + 1
        end
    end
    return nCount
end

function onEnterTrigger(nGameId, nTriggerId)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then return end
    procNodeGraphEvent(tSvrGame, Const.EventType.EnterTrigger, nTriggerId)
end

function onAttackHit(nGameId, nAttackerId, nTargetId)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then return end
    local tAttacker = tSvrGame.tGameObjects[nAttackerId]
    if tAttacker == nil then return end
    local tTarget = tSvrGame.tGameObjects[nTargetId]
    if tTarget == nil then return end
    local nAtk = tAttacker.tConfig.nAtk
    tTarget.nCurrHP = tTarget.nCurrHP - nAtk
    if tTarget.nCurrHP <= 0 then
        tTarget.nCurrHP = 0
        SvrGameMod.roleDead(tSvrGame, nTargetId)
    end
end