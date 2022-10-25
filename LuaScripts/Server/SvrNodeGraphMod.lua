
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
    tNodeGraph.nState = Const.NodeGraphState.Pending
    tNodeGraph.tEventListeners = {}
    tNodeGraph.tFinishedNodes = {}
    tNodeGraph.bServer = true
    NodeGraphEventMod.init(tNodeGraph)
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end

    tNodeGraph.nState = Const.NodeGraphState.Running
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

    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if tSvrGame ~= nil then
        local nGameId = tSvrGame.nGameId
        Messager.S2CFinishNode(nGameId, tNodeGraph.nNodeGraphId, nNodeId, nPath)
    end
    tNodeGraph.tFinishedNodes[nNodeId] = true

    NodeGraphEventMod.processEvent(tNodeGraph, Const.EventType.FinishNode, nNodeId)

    local tTransitionNodes = NodeGraphCfgMod.getTransitionNodes(tNodeGraph.tConfigData, nNodeId, nPath)
    if tTransitionNodes ~= nil then
        for _, sToNodeId in ipairs(tTransitionNodes) do
            SvrNodeGraphMod.triggerNode(tNodeGraph, sToNodeId)
        end
    end

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
        if tSvrGame ~= nil then
            Messager.S2CFinishNodeGraph(tSvrGame.nGameId, tNodeGraph.nNodeGraphId)
        end
    end
end

function isFinish(tNodeGraph)
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

