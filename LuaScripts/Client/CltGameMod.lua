
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

local function procNodeGraphEvent(nEventType, ...)
    if tCltGame == nil then
        return
    end
    local tNodeGraph = tCltGame.tMainNodeGraph
    if tNodeGraph ~= nil then
        CltNodeGraphMod.processEvent(tNodeGraph, nEventType, ...)
    end
end

function addPlayer(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, bLocalPlayer)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tConfig = Config.Player[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local tPlayer = {}
    tPlayer.nObjectId = nObjectId
    tPlayer.nObjectType = Const.GameObjectType.Player
    tPlayer.tConfig = tConfig
    tCltGame.tGameObjects[nObjectId] = tPlayer
    local nModelId = tConfig.nModelId
    local tModelConfig = Config.Model[nModelId]
    if tModelConfig ~= nil then
        local sPrefabFile = tModelConfig.sPrefabFile
        local goPrefab = UE.Resources.Load(sPrefabFile)
        local goInstance = UE.GameObject.Instantiate(goPrefab)
        tPlayer.goInstance = goInstance
        local goPlayerRoot = UE.GameObject.Find("GameObjects/Players")
        if goPlayerRoot ~= nil then
            goInstance.transform:SetParent(goPlayerRoot.transform)
        end
        goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
        local uGameObjectId = goInstance:GetComponent(typeof(CS.Game.GameObjectId))
        if uGameObjectId ~= nil then
            uGameObjectId.ID = nObjectId
        end
    end
    if bLocalPlayer then
        tPlayer.bLocalPlayer = true
        CltLocalPlayerMod.setLocalPlayer(tPlayer)
    end
end

function addMonster(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, sRefreshId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tConfig = Config.Monster[nConfigId]
    if tConfig == nil then
        printError("MonsterConfig not exist configId: " .. nConfigId)
        return
    end
    local tMonster = {}
    tMonster.nObjectId = nObjectId
    tMonster.nObjectType = Const.GameObjectType.Monster
    tMonster.tConfig = tConfig
    tMonster.sRefreshId = sRefreshId
    tCltGame.tGameObjects[nObjectId] = tMonster
    local nModelId = tConfig.nModelId
    local tModelConfig = Config.Model[nModelId]
    if tModelConfig ~= nil then
        local sPrefabFile = tModelConfig.sPrefabFile
        local goPrefab = UE.Resources.Load(sPrefabFile)
        local goInstance = UE.GameObject.Instantiate(goPrefab)
        tMonster.goInstance = goInstance
        local goMonsterRoot = UE.GameObject.Find("GameObjects/Monsters")
        if goMonsterRoot ~= nil then
            goInstance.transform:SetParent(goMonsterRoot.transform)
        end
        goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
        local uGameObjectId = goInstance:GetComponent(typeof(CS.Game.GameObjectId))
        if uGameObjectId ~= nil then
            uGameObjectId.ID = nObjectId
        end
    end
    procNodeGraphEvent(Const.EventType.AddMonster)
end

function roleDead(nGameId, nObjectId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tGameObject = tCltGame.tGameObjects[nObjectId]
    if tGameObject == nil then
        return
    end
    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        procNodeGraphEvent(Const.EventType.MonsterDead, nObjectId)
    end
    
    CltAnimatorMod.roleDead(tGameObject, function()
        local goInstance = tGameObject.goInstance
        if goInstance ~= nil then
            UE.GameObject.Destroy(goInstance)
        end
    end)
    tCltGame.tGameObjects[nObjectId] = nil
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

function getMonsterByRefreshId(sRefreshId)
    if tCltGame == nil then
        return
    end
    for _, tGameObject in pairs(tCltGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            if tGameObject.sRefreshId == sRefreshId then
                return tGameObject
            end
        end
    end
end

function onTriggerEnter(nTriggerId, uCollider)
    if uCollider.gameObject:GetComponent(typeof(CS.Game.PlayerCollider)) == nil then
        return
    end

    if tCltGame ~= nil then
        procNodeGraphEvent(Const.EventType.EnterTrigger, nTriggerId)
        Messager.C2SEnterTrigger(tCltGame.nGameId, nTriggerId)
    end
end

function onAttackHit(nAttackerId, nTargetId)
    if tCltGame == nil then
        return 
    end
    Messager.C2SAttackHit(tCltGame.nGameId, nAttackerId, nTargetId)
end
