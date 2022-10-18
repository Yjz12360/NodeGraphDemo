
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
    if tSvrGame == nil then
        return
    end
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
    if tSvrGame == nil then
        return
    end
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
    return tMonster
end

function addSceneMonster(tSvrGame, nRefreshId)
    if tSvrGame == nil then
        return
    end
    if SvrGameMod.getMonsterByRefreshId(tSvrGame, nRefreshId) ~= nil then
        printError("addSceneMonster error: monster already exists. nRefreshId: " .. nRefreshId)
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshConfig = GameSceneCfgMod.getRefreshMonsterConfig(tGameSceneConfig, nRefreshId)
    if tRefreshConfig == nil then
        printError("addSceneMonster error: tRefreshConfig not exist. nRefreshId: " .. nRefreshId)
        return
    end
    local nMonsterCfgId = tRefreshConfig.nMonsterCfgId
    local tPos = tRefreshConfig.tPos
    local tMonster = SvrGameMod.addMonster(tSvrGame, nMonsterCfgId, tPos.x, tPos.y, tPos.z)
    tMonster.nRefreshId = nRefreshId
    return tMonster
end

function refreshMonsterGroup(tSvrGame, nRefreshId)
    if tSvrGame == nil then
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshMonsterGroups = GameSceneCfgMod.getRefreshMonsterGroup(tGameSceneConfig, nRefreshId)
    if tRefreshMonsterGroups == nil then
        printError("refreshMonsterGroup error: config not exist. nRefreshId: " .. nRefreshId)
        return
    end
    for _, nRefreshMonsterId in ipairs(tRefreshMonsterGroups) do
        local tMonster = SvrGameMod.addSceneMonster(tSvrGame, nRefreshMonsterId)
        tMonster.nGroupId = nRefreshId
    end
end

function roleDead(tSvrGame, nObjectId)
    if tSvrGame == nil then
        return
    end
    local tRole = tSvrGame.tGameObjects[nObjectId]
    if tRole == nil then
        return
    end

    local tGameObject = tSvrGame.tGameObjects[nObjectId]
    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        procNodeGraphEvent(tSvrGame, Const.EventType.BeforeMonsterDead, nObjectId)
    end

    tSvrGame.tGameObjects[nObjectId] = nil
    Messager.S2CRoleDead(tSvrGame.nGameId, nObjectId)

    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        procNodeGraphEvent(tSvrGame, Const.EventType.MonsterDead, nObjectId)
    end
end

function hasMonster(tSvrGame)
    if tSvrGame == nil then
        return false
    end
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            return true
        end
    end
    return false
end

function getMonsterNum(tSvrGame)
    if tSvrGame == nil then
        return 0
    end
    local nCount = 0
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            nCount = nCount + 1
        end
    end
    return nCount
end

function getMonsterByRefreshId(tSvrGame, nRefreshId)
    if tSvrGame == nil then
        return
    end
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            if tGameObject.nRefreshId == nRefreshId then
                return tGameObject
            end
        end
    end
end

function getObject(tSvrGame, nObjectId)
    if tSvrGame == nil then
        return
    end
    return tSvrGame.tGameObjects[nObjectId]
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