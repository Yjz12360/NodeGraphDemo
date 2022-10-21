
local function addEventListener(tNodeGraph, tNodeData, nEventType)
    local tEventListeners = tNodeGraph.tEventListeners
    if tEventListeners == nil then
        tEventListeners = {}
        tNodeGraph.tEventListeners = tEventListeners
    end
    local tListeners = tEventListeners[nEventType]
    if tListeners == nil then
        tListeners = {}
        tEventListeners[nEventType] = tListeners
    end
    tListeners[tNodeData.sNodeId] = true
end

local function delEventListener(tNodeGraph, tNodeData, nEventType)
    local tEventListeners = tNodeGraph.tEventListeners
    if tEventListeners == nil then
        return
    end
    local tListeners = tEventListeners[nEventType]
    if tListeners == nil then
        return
    end
    tListeners[tNodeData.sNodeId] = nil
end

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
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end

    tNodeGraph.nState = Const.NodeGraphState.Running
    local sStartNodeId = tNodeGraph.tConfigData.sStartNodeId
    SvrNodeGraphMod.finishNode(tNodeGraph, sStartNodeId)
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

    local fNodeHandler = NodesHandlerMod.getSvrNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        fNodeHandler(tNodeGraph, tNodeData)
    end

    local tSvrEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
    if tSvrEvents ~= nil then
        for nEventType, _ in pairs(tSvrEvents) do
            addEventListener(tNodeGraph, tNodeData, nEventType)
        end
    end

    if fNodeHandler == nil and tSvrEvents == nil then
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
    if tNodeGraph.tFinishedNodes[sNodeId] then
        return
    end

    nPath = nPath or 1

    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if tSvrGame ~= nil then
        local nGameId = tSvrGame.nGameId
        Messager.S2CFinishNode(nGameId, tNodeGraph.nNodeGraphId, sNodeId, nPath)
    end
    tNodeGraph.tFinishedNodes[sNodeId] = true

    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData ~= nil then
        local tSvrEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
        if tSvrEvents ~= nil then
            for nEventType, _ in pairs(tSvrEvents) do
                delEventListener(tNodeGraph, tNodeData, nEventType)
            end
        end
    end

    SvrNodeGraphMod.processEvent(tNodeGraph, Const.EventType.FinishNode, sNodeId)

    local tTransitions = NodeGraphCfgMod.getTransitions(tNodeGraph.tConfigData)
    for _, tTransition in pairs(tTransitions) do
        if tTransition.sFromNodeId == sNodeId and tTransition.nPath == nPath then
            SvrNodeGraphMod.triggerNode(tNodeGraph, tTransition.sToNodeId)
        end
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
        tNodeGraph.nState = Const.NodeGraphState.Finish
        if tSvrGame ~= nil then
            Messager.S2CFinishNodeGraph(tSvrGame.nGameId, tNodeGraph.nNodeGraphId)
        end
    end
end

function processEvent(tNodeGraph, nEventType, ...)
    local tEventListeners = tNodeGraph.tEventListeners
    if tEventListeners == nil then 
        return
    end
    local tListeners = tEventListeners[nEventType]
    if tListeners == nil then
        return
    end
    for sNodeId, _ in pairs(tListeners) do
        if not tNodeGraph.tFinishedNodes[sNodeId] then
            local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
            local nNodeType = tNodeData.nNodeType
            local fEventHandler = NodesHandlerMod.getSvrEventHandler(nNodeType, nEventType)
            if fEventHandler ~= nil then
                fEventHandler(tNodeGraph, tNodeData, ...)
            end
        end
    end
end

function isFinish(tNodeGraph)
    return tNodeGraph.nState == Const.NodeGraphState.Finish
end

