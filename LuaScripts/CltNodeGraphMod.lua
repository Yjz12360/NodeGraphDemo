
function addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    local tConfig = Config.NodeGraph[nConfigId]
    if tConfig == nil or tConfig.sName == nil then
        printError("addNodeGraph error nConfigId: " .. nConfigId)
        return
    end
    local tNodeGraph = {}
    tNodeGraph.tCltGame = tCltGame
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
    if tNodeData == nil then
        return
    end
    if tNodeData.nNodeType == Const.NodeType.Start then
        CltNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end

    local fNodeHandler = NodesHandlerMod.getCltNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        local tHandlers = fNodeHandler(tNodeGraph, tNodeData)
        if tHandlers ~= nil then
            tNodeGraph.tNodeHandlers[sNodeId] = tHandlers
            if tHandlers.fUpdate ~= nil then
                tNodeGraph.tActiveNodeState[sNodeId] = Const.NodeState.Running
            else
                tNodeGraph.tActiveNodeState[sNodeId] = Const.NodeState.Hanging
            end
        end
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
            CltNodeGraphMod.triggerNode(tNodeGraph, sNodeId)
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
    if tNodeGraph.tFinishedNodes[sNodeId] then
        return
    end
    if tNodeGraph.tActiveNodeState[sNodeId] == nil then
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
    end
end

function isFinish(tNodeGraph)
    if tNodeGraph == nil then
        return false
    end
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

function handleEnterTrigger(tNodeGraph, nTriggerId)
    for sNodeId, tHandler in pairs(tNodeGraph.tNodeHandlers) do
        local fTriggerEnter = tHandler.fTriggerEnter
        if fTriggerEnter then
            fTriggerEnter(nTriggerId)
        end
    end
end

function recvFinishNode(nGameId, nNodeGraphId, sNodeId, nPath)
    local tCltGame = CltGameMod.getGame()
    if tCltGame == nil then
        return
    end
    if tCltGame.nGameId ~= nCurrGameId then
        return
    end
    local tMainNodeGraph = tCltGame.tMainNodeGraph
    if tMainNodeGraph == nil or tMainNodeGraph.nNodeGraphId ~= nNodeGraphId then
        return
    end
    CltNodeGraphMod.finishNode(tMainNodeGraph, sNodeId, nPath)
end

function recvFinishNodeGraph(nGameId, nNodeGraphId)
    local tCltGame = CltGameMod.getGame()
    if tCltGame == nil then
        return
    end
    if tCltGame.nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = tCltGame.tMainNodeGraph
    if tNodeGraph == nil or tNodeGraph.nNodeGraphId ~= nNodeGraphId then
        return
    end
    TableUtil.clear(tNodeGraph.tActiveNodeState)
    TableUtil.clear(tNodeGraph.tNodeHandlers)
    tNodeGraph.nState = Const.NodeGraphState.Finish
end
