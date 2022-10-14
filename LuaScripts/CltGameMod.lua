
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
    tCltGame.tMainNodeGraph = nil
end

function isLocalGame()
    return true
end

function getGameId()
    return nCurrGameId
end

function getGame()
    return tCltGame
end

function addNodeGraph(nGameId, nNodeGraphId, nConfigId)
    if nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = CltNodeGraphMod.addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    tCltGame.tMainNodeGraph = tNodeGraph
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

function onTriggerEnter(goSource, uCollider)
    local nTriggerId = tonumber(goSource.name)
    if nTriggerId == nil then
        printError("Game Trigger Id error: " .. goSource.name)
        return
    end
    local transParent = goSource.transform.parent
    if transParent == nil then
        return
    end
    if transParent:GetComponent(typeof(CS.Game.GameTriggerContainer)) == nil then
        return
    end
    if uCollider.gameObject:GetComponent(typeof(CS.Game.PlayerCollider)) == nil then
        return
    end

    
    if tCltGame ~= nil then
        Messager.C2SEnterTrigger(tCltGame.nGameId, nTriggerId)
        -- local tMainNodeGraph = tCltGame.tMainNodeGraph
        -- if tMainNodeGraph ~= nil then
        --     CltNodeGraphMod.onPlayerEnterTrigger(tMainNodeGraph, nTriggerId)
        -- end
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

-- function monsterMove(nObjectId, nPosX, nPosY, nPosZ)

-- end

-- function monsterChase(nMonsterId, nTargetId, nChaseTime, nStopDistance)

-- end


-- function removeObject(nObjectId)

-- end
