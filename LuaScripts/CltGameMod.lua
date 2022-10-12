
local nCurrGameId = 0
local tCltGame = nil

function init()
    local nGameConfigId = 1
    Messager.C2SCreateGame(nGameConfigId)
end

function recvCreateGameSucc(nGameId)
    nCurrGameId = nGameId
    CltGameMod.createGame()
end

function createGame()
    tCltGame = {}
    tCltGame.tGameObjects = {}
    tCltGame.tNodeGraphs = {}
end

function isLocalGame()
    return true
end

function getGameId()
    return nCurrGameId
end

function addNodeGraph(nGameId, nNodeGraphId, nConfigId)
    if nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = CltNodeGraphMod.addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    tCltGame.tNodeGraphs[nNodeGraphId] = tNodeGraph
    CltNodeGraphMod.startNodeGraph(tNodeGraph)
end

function addPlayer(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
    if nGameId ~= nCurrGameId then
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

function update(nDeltaTime)
    if tCltGame == nil then
        return
    end
    for nNodeGraphId, tNodeGraph in pairs(tCltGame.tNodeGraphs) do
        CltNodeGraphMod.updateNodeGraph(tNodeGraph, nDeltaTime)
        if CltNodeGraphMod.isFinish(tNodeGraph) then
            tCltGame.tNodeGraphs[nNodeGraphId] = nil
        end
    end
end

function recvFinishNode(nGameId, nNodeGraphId, sNodeId, nPath)
    if nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = tCltGame.tNodeGraphs[nNodeGraphId]
    if tNodeGraph == nil then
        return
    end
    CltNodeGraphMod.finishNode(tNodeGraph, sNodeId, nPath)
end

function recvFinishNodeGraph(nGameId, nNodeGraphId)
    if nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = tCltGame.tNodeGraphs[nNodeGraphId]
    if tNodeGraph == nil then
        return
    end
    CltNodeGraphMod.setFinish(tNodeGraph)
end

-- function monsterMove(nObjectId, nPosX, nPosY, nPosZ)

-- end

-- function monsterChase(nMonsterId, nTargetId, nChaseTime, nStopDistance)

-- end


-- function removeObject(nObjectId)

-- end
