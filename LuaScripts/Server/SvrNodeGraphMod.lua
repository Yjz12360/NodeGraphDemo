
function addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    local tConfig = Config.NodeGraph[nConfigId]
    if tConfig == nil or tConfig.sName == nil then
        printError("addNodeGraph error nConfigId: " .. nConfigId)
        return
    end
    local tNodeGraph = {}
    tNodeGraph.nGameId = tSvrGame.nGameId
    tNodeGraph.nNodeGraphId = nNodeGraphId
    tNodeGraph.tConfigData = NodeGraphCfgMod.getConfigByName(tConfig.sName)
    tNodeGraph.tFinishedNodes = {}
    tNodeGraph.bServer = true
    NodeGraphEventMod.init(tNodeGraph)
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end

    local nStartNodeId = tNodeGraph.tConfigData.nStartNodeId
    SvrNodeGraphMod.finishNode(tNodeGraph, nStartNodeId)
end

function triggerNode(tNodeGraph, nNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
    if tNodeData == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId)
        return
    end

    local fNodeHandler = NodesHandlerMod.getSvrNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        fNodeHandler(tNodeGraph, tNodeData)
    end

    if fNodeHandler == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId)
    end
end

function finishNode(tNodeGraph, nNodeId, bSync, nPath)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.tFinishedNodes[nNodeId] then
        return
    end

    nPath = nPath or 1
    
    if bSync then
        local nGameId = tNodeGraph.nGameId
        if SvrGameMod.getGameById(nGameId) ~= nil then
            Messager.S2CFinishNode(nGameId, tNodeGraph.nNodeGraphId, nNodeId, nPath)
        end
    end
    tNodeGraph.tFinishedNodes[nNodeId] = true

    NodeGraphEventMod.processEvent(tNodeGraph, Const.EventType.FinishNode, nNodeId)

    local tTransitionNodes = NodeGraphCfgMod.getTransitionNodes(tNodeGraph.tConfigData, nNodeId, nPath)
    if tTransitionNodes ~= nil then
        for _, sToNodeId in ipairs(tTransitionNodes) do
            SvrNodeGraphMod.triggerNode(tNodeGraph, sToNodeId)
        end
    end
end
