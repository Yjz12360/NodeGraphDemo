
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

    local nStartNodeId = tNodeGraph.tConfigData.nStartNodeId
    CltNodeGraphMod.finishNode(tNodeGraph, nStartNodeId)
end

function triggerNode(tNodeGraph, nNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
    if tNodeData == nil then
        CltNodeGraphMod.finishNode(tNodeGraph, nNodeId)
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