
local tCltGame = nil
local nGameConfigId

function init()
    nGameConfigId = 1
    Messager.C2SCreateGame(nGameConfigId)
end

function recvCreateGameSucc(nGameId)
    CltGameMod.createGame(nGameId, nGameConfigId)
end

function createGame(nGameId, nGameConfigId)
    local tGameConfig = Config.Game[nGameConfigId]
    if tGameConfig == nil then
        printError("createGame error: game config not exists. config id: " .. nGameConfigId)
        return
    end
    local sSceneConfig = tGameConfig.sSceneConfig
    local tGameSceneConfig = GameSceneCfgMod.getConfigByName(sSceneConfig)
    if tGameSceneConfig == nil then
        printError("GameSceneConfig not exist config: " .. sSceneConfig)
        return
    end

    tCltGame = {}
    tCltGame.nGameId = nGameId
    tCltGame.tGameObjects = {}
    tCltGame.tGameConfig = tGameConfig
    tCltGame.tGameSceneConfig = tGameSceneConfig
    tCltGame.tMainNodeGraph = nil

    local sSceneConfig = tGameConfig.sSceneConfig
    local goDataPrefab = UE.Resources.Load("SceneData/" .. sSceneConfig)
    if goDataPrefab ~= nil then
        local goInstance = UE.GameObject.Instantiate(goDataPrefab)
        goInstance.name = "SceneData"
    end
end

function isLocalGame()
    return true
end

function checkGameId(nGameId)
    if tCltGame == nil then
        return false
    end
    return tCltGame.nGameId == nGameId
end

function getGame()
    return tCltGame
end

function getGameId()
    if tCltGame == nil then
        return 0
    end
    return tCltGame.nGameId
end

function addNodeGraph(nGameId, nNodeGraphId, nConfigId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tNodeGraph = CltNodeGraphMod.addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    tCltGame.tMainNodeGraph = tNodeGraph
    CltNodeGraphMod.startNodeGraph(tNodeGraph)
end

function processEvent(nEventType, ...)
    if tCltGame == nil then
        return
    end
    local tNodeGraph = tCltGame.tMainNodeGraph
    if tNodeGraph ~= nil then
        CltNodeGraphMod.processEvent(tNodeGraph, nEventType, ...)
    end
end

function getObject(nObjectId)
    if tCltGame == nil then
        return
    end
    return tCltGame.tGameObjects[nObjectId]
end

function getObjects()
    if tCltGame == nil then
        return
    end
    return tCltGame.tGameObjects
end



function onTriggerEnter(nTriggerId, uCollider)
    if uCollider.gameObject:GetComponent(typeof(CS.Game.PlayerCollider)) == nil then
        return
    end

    if tCltGame ~= nil then
        CltGameMod.processEvent(Const.EventType.EnterTrigger, nTriggerId)
        Messager.C2SEnterTrigger(tCltGame.nGameId, nTriggerId)
    end
end

function onAttackHit(nAttackerId, nTargetId)
    if tCltGame == nil then
        return 
    end
    Messager.C2SAttackHit(tCltGame.nGameId, nAttackerId, nTargetId)
end
