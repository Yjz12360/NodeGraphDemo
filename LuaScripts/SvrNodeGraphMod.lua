
function addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    local tConfig = Config.NodeGraph[nConfigId]
    if tConfig == nil or tConfig.sName == nil then
        printError("addNodeGraph error nConfigId: " .. nConfigId)
        return
    end
    local tNodeGraph = {}
    tNodeGraph.tSvrGame = tSvrGame
    tNodeGraph.nNodeGraphId = nNodeGraphId
    tNodeGraph.tConfigData = NodeGraphCfgMod.getConfigByName(tConfig.sName)
    tNodeGraph.nState = Const.NodeGraphState.Pending
    tNodeGraph.tActiveNodeState = {}
    tNodeGraph.tNodeHandlers = {}
    tNodeGraph.tFinishedNodes = {}
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end
    TableUtil.clear(tNodeGraph.tActiveNodeState)
    TableUtil.clear(tNodeGraph.tNodeHandlers)
    TableUtil.clear(tNodeGraph.tFinishedNodes)

    tNodeGraph.nState = Const.NodeGraphState.Running
    local sStartNodeId = tNodeGraph.tConfigData.sStartNodeId
    tNodeGraph.tActiveNodeState[sStartNodeId] = Const.NodeState.Pending
end

function triggerNode(tNodeGraph, sNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData == nil or tNodeData.nNodeType == Const.NodeType.Start then
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end
    local fNodeHandler = NodesHandlerMod.getSvrNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        local tHandlers = fNodeHandler(tNodeGraph, tNodeData)
        if tHandlers ~= nil then
            tNodeGraph.tNodeHandlers[sNodeId] = tHandlers
            if tHandlers.fUpdate ~= nil then
                tNodeGraph.tActiveNodeState[sNodeId] = Const.NodeState.Running
            else
                tNodeGraph.tActiveNodeState[sNodeId] = Const.NodeState.Hanging
            end
        else
            SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        end
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
    end
end

function updateNodeGraph(tNodeGraph, nDeltaTime)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= Const.NodeGraphState.Running then
        return
    end
    for sNodeId, nNodeState in pairs(tNodeGraph.tActiveNodeState) do
        if nNodeState == Const.NodeState.Activated then
            tNodeGraph.tActiveNodeState[sNodeId] = Const.NodeState.Pending
        end
    end
    for sNodeId, nNodeState in pairs(tNodeGraph.tActiveNodeState) do
        if nNodeState == Const.NodeState.Pending then
            SvrNodeGraphMod.triggerNode(tNodeGraph, sNodeId)
        end
    end
    for sNodeId, nNodeState in pairs(tNodeGraph.tActiveNodeState) do
        if nNodeState == Const.NodeState.Running then
            local tHandlers = tNodeGraph.tNodeHandlers[sNodeId]
            if tHandlers ~= nil then
                local fUpdate = tHandlers.fUpdate
                if fUpdate ~= nil then
                    fUpdate(nDeltaTime)
                end
            end
        end
    end
end

function finishNode(tNodeGraph, sNodeId, nPath)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= Const.NodeGraphState.Running then
        return
    end
    nPath = nPath or 1
    local tTransitions = NodeGraphCfgMod.getTransitions(tNodeGraph.tConfigData)
    for _, tTransition in pairs(tTransitions) do
        if tTransition.sFromNodeId == sNodeId and tTransition.nPath == nPath then
            tNodeGraph.tActiveNodeState[tTransition.sToNodeId] = Const.NodeState.Activated
        end
    end
    tNodeGraph.tActiveNodeState[sNodeId] = nil
    tNodeGraph.tNodeHandlers[sNodeId] = nil
    tNodeGraph.tFinishedNodes[sNodeId] = true

    local tSvrGame = tNodeGraph.tSvrGame
    if tSvrGame ~= nil then
        local nGameId = tSvrGame.nGameId
        Messager.S2CFinishNode(nGameId, tNodeGraph.nNodeGraphId, sNodeId, nPath)
    end

    local bAllFinish = true
    local tNodeMap = tNodeGraph.tConfigData.tNodeMap
    for sNodeId, _ in pairs(tNodeMap) do
        if not tNodeGraph.tFinishedNodes[sNodeId] then
            bAllFinish = false
            break
        end
    end
    if bAllFinish then
        TableUtil.clear(tNodeGraph.tActiveNodeState)
        TableUtil.clear(tNodeGraph.tNodeHandlers)
        tNodeGraph.nState = Const.NodeGraphState.Finish
        if tSvrGame ~= nil then
            Messager.S2CFinishNodeGraph(tSvrGame.nGameId, tNodeGraph.nNodeGraphId)
        end
    end
end

function handleEnterTrigger(tNodeGraph, nTriggerId)
    if tNodeGraph == nil then
        return 
    end
    for sNodeId, tHandler in pairs(tNodeGraph.tNodeHandlers) do
        local fTriggerEnter = tHandler.fTriggerEnter
        if fTriggerEnter then
            fTriggerEnter(nTriggerId)
        end
    end
end

function isFinish(tNodeGraph)
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

