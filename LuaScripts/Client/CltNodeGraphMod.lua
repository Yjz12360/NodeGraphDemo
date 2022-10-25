
function addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    local tConfig = Config.NodeGraph[nConfigId]
    if tConfig == nil or tConfig.sName == nil then
        printError("addNodeGraph error nConfigId: " .. nConfigId)
        return
    end
    local tNodeGraph = {}
    tNodeGraph.nGameId = tCltGame.nGameId
    tNodeGraph.nNodeGraphId = nNodeGraphId
    tNodeGraph.tConfigData = NodeGraphCfgMod.getConfigByName(tConfig.sName)
    tNodeGraph.nState = Const.NodeGraphState.Pending
    tNodeGraph.tEventListeners = {}
    tNodeGraph.tFinishedNodes = {}
    NodeGraphEventMod.init(tNodeGraph)
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end
    TableUtil.clear(tNodeGraph.tFinishedNodes)

    tNodeGraph.nState = Const.NodeGraphState.Running
    local nStartNodeId = tNodeGraph.tConfigData.nStartNodeId
    CltNodeGraphMod.finishNode(tNodeGraph, nStartNodeId)
end

function triggerNode(tNodeGraph, nNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
    if tNodeData == nil then
        return
    end

    local fNodeHandler = NodesHandlerMod.getCltNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        fNodeHandler(tNodeGraph, tNodeData)
    end
end

function finishNode(tNodeGraph, nNodeId, nPath)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= Const.NodeGraphState.Running then
        return
    end
    if tNodeGraph.tFinishedNodes[nNodeId] then
        return
    end

    nPath = nPath or 1
    local tTransitionNodes = NodeGraphCfgMod.getTransitionNodes(tNodeGraph.tConfigData, nNodeId, nPath)
    if tTransitionNodes ~= nil then
        for _, sToNodeId in ipairs(tTransitionNodes) do
            CltNodeGraphMod.triggerNode(tNodeGraph, sToNodeId)
        end
    end

    tNodeGraph.tFinishedNodes[nNodeId] = true

    local bAllFinish = true
    local tNodeMap = tNodeGraph.tConfigData.tNodeMap
    for nNodeId, _ in pairs(tNodeMap) do
        if not tNodeGraph.tFinishedNodes[nNodeId] then
            bAllFinish = false
            break
        end
    end
    if bAllFinish then
        tNodeGraph.nState = Const.NodeGraphState.Finish
    end
end

function isFinish(tNodeGraph)
    if tNodeGraph == nil then
        return false
    end
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

function recvFinishNode(nGameId, nNodeGraphId, nNodeId, nPath)
    local tCltGame = CltGameMod.getGame()
    if tCltGame == nil then
        return
    end
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tMainNodeGraph = tCltGame.tMainNodeGraph
    if tMainNodeGraph == nil or tMainNodeGraph.nNodeGraphId ~= nNodeGraphId then
        return
    end
    CltNodeGraphMod.finishNode(tMainNodeGraph, nNodeId, nPath)
end

function recvFinishNodeGraph(nGameId, nNodeGraphId)
    local tCltGame = CltGameMod.getGame()
    if tCltGame == nil then
        return
    end
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tNodeGraph = tCltGame.tMainNodeGraph
    if tNodeGraph == nil or tNodeGraph.nNodeGraphId ~= nNodeGraphId then
        return
    end
    tNodeGraph.nState = Const.NodeGraphState.Finish
end
