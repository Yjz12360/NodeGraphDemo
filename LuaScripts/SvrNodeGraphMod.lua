
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
    tNodeGraph.tRunningNodes = {}
    tNodeGraph.tPendingNodes = {}
    tNodeGraph.tRemoveNodes = {}
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end
    TableUtil.clear(tNodeGraph.tPendingNodes)
    TableUtil.clear(tNodeGraph.tRemoveNodes)
    tNodeGraph.nState = Const.NodeGraphState.Running
    local sStartNodeId = tNodeGraph.tConfigData.sStartNodeId
    tNodeGraph.tPendingNodes[sStartNodeId] = true
end

function updateNodeGraph(tNodeGraph, nDeltaTime)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= Const.NodeGraphState.Running then
        return
    end
    local tClonePendingNodes = {}
    for sNodeId, _ in pairs(tNodeGraph.tPendingNodes) do
        tClonePendingNodes[sNodeId] = true
    end
    TableUtil.clear(tNodeGraph.tPendingNodes)
    for sNodeId, _ in pairs(tClonePendingNodes) do
        SvrNodeGraphMod.triggerNode(tNodeGraph, sNodeId)
    end
    for sNodeId, _ in pairs(tNodeGraph.tRunningNodes) do
        local fUpdate = tNodeGraph.tRunningNodes[sNodeId]
        if fUpdate ~= nil then
            fUpdate(nDeltaTime)
        end
    end
    for sNodeId, _ in pairs(tNodeGraph.tRemoveNodes) do
        tNodeGraph.tRunningNodes[sNodeId] = nil
    end
    TableUtil.clear(tNodeGraph.tRemoveNodes)
    if(TableUtil.isEmpty(tNodeGraph.tRunningNodes) and TableUtil.isEmpty(tNodeGraph.tPendingNodes)) then
        tNodeGraph.nState = Const.NodeGraphState.Finish
        local tSvrGame = tNodeGraph.tSvrGame
        if tSvrGame ~= nil then
            Messager.S2CFinishNodeGraph(tSvrGame.nGameId, tNodeGraph.nNodeGraphId)
        end
    end
end

function triggerNode(tNodeGraph, sNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end
    if tNodeData.nNodeType == Const.NodeType.Start then
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end
    local fHandler = NodesHandlerMod.getSvrNodeHandler(tNodeData.nNodeType)
    if fHandler ~= nil then
        local tFuncs = fHandler(tNodeGraph, tNodeData)
        if tFuncs ~= nil then
            local fUpdate = tFuncs.fUpdate
            if fUpdate ~= nil then
                tNodeGraph.tRunningNodes[sNodeId] = fUpdate
            end
        end
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
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
    tNodeGraph.tRemoveNodes[sNodeId] = true
    local tTransitions = NodeGraphCfgMod.getTransitions(tNodeGraph.tConfigData)
    for _, tTransition in pairs(tTransitions) do
        if tTransition.sFromNodeId == sNodeId and tTransition.nPath == nPath then
            tNodeGraph.tPendingNodes[tTransition.sToNodeId] = true
        end
    end
    local nGameId = tNodeGraph.tSvrGame.nGameId
    Messager.S2CFinishNode(nGameId, tNodeGraph.nNodeGraphId, sNodeId, nPath)
end

function handleEnterTrigger(tNodeGraph, nTriggerId)
    if tNodeGraph == nil then
        return 
    end
    print("99999")
    -- for sNodeId, _ in pairs(tNodeGraph.tRunningNodes) do
    --     -- local fUpdate = tNodeGraph.tRunningNodes[sNodeId]
    --     -- if fUpdate ~= nil then
    --     --     fUpdate(nDeltaTime)
    --     -- end
    -- end
end

function isFinish(tNodeGraph)
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

