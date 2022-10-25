
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
    tListeners[tNodeData.nNodeId] = true
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
    tListeners[tNodeData.nNodeId] = nil
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

    local tSvrEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
    if tSvrEvents ~= nil then
        for nEventType, _ in pairs(tSvrEvents) do
            addEventListener(tNodeGraph, tNodeData, nEventType)
        end
    end

    if fNodeHandler == nil and tSvrEvents == nil then
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

    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
    if tNodeData ~= nil then
        local tSvrEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
        if tSvrEvents ~= nil then
            for nEventType, _ in pairs(tSvrEvents) do
                delEventListener(tNodeGraph, tNodeData, nEventType)
            end
        end
    end

    SvrNodeGraphMod.processEvent(tNodeGraph, Const.EventType.FinishNode, nNodeId)

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

function processEvent(tNodeGraph, nEventType, ...)
    local tEventListeners = tNodeGraph.tEventListeners
    if tEventListeners == nil then 
        return
    end
    local tListeners = tEventListeners[nEventType]
    if tListeners == nil then
        return
    end
    for nNodeId, _ in pairs(tListeners) do
        if not tNodeGraph.tFinishedNodes[nNodeId] then
            local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
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

