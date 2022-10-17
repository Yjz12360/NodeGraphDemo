
local tCltGame = nil

function init()
    local nGameConfigId = 1
    Messager.C2SCreateGame(nGameConfigId)
end

function recvCreateGameSucc(nGameId)
    CltGameMod.createGame(nGameId)
end

function createGame(nGameId)
    tCltGame = {}
    tCltGame.nGameId = nGameId
    tCltGame.tGameObjects = {}
    tCltGame.tMainNodeGraph = nil
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

function addNodeGraph(nGameId, nNodeGraphId, nConfigId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tNodeGraph = CltNodeGraphMod.addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    tCltGame.tMainNodeGraph = tNodeGraph
    CltNodeGraphMod.startNodeGraph(tNodeGraph)
end

function addPlayer(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
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
    end
end

function addMonster(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
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
    end
end

function onTriggerEnter(nTriggerId, uCollider)
    if uCollider.gameObject:GetComponent(typeof(CS.Game.PlayerCollider)) == nil then
        return
    end

    if tCltGame ~= nil then
        local tNodeGraph = tCltGame.tMainNodeGraph
        if tNodeGraph ~= nil then
            CltNodeGraphMod.processEvent(tNodeGraph, Const.EventType.EnterTrigger, nTriggerId)
        end
        Messager.C2SEnterTrigger(tCltGame.nGameId, nTriggerId)
    end
end

function update(nDeltaTime)
    if tCltGame == nil then
        return
    end
    local tMainNodeGraph = tCltGame.tMainNodeGraph
    if tMainNodeGraph ~= nil then
        CltNodeGraphMod.updateNodeGraph(tMainNodeGraph, nDeltaTime)
        if CltNodeGraphMod.isFinish(tMainNodeGraph) then
            tCltGame.tMainNodeGraph = nil
        end
    end
end

