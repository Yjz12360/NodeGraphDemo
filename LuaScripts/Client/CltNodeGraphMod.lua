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
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end
    TableUtil.clear(tNodeGraph.tFinishedNodes)

    tNodeGraph.nState = Const.NodeGraphState.Running
    local sStartNodeId = tNodeGraph.tConfigData.sStartNodeId
    CltNodeGraphMod.finishNode(tNodeGraph, sStartNodeId)
end

function triggerNode(tNodeGraph, sNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData == nil then
        return
    end

    local fNodeHandler = NodesHandlerMod.getCltNodeHandler(tNodeData.nNodeType)
    if fNodeHandler ~= nil then
        fNodeHandler(tNodeGraph, tNodeData)
    end
    local tCltEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
    if tCltEvents ~= nil then
        for nEventType, _ in pairs(tCltEvents) do
            addEventListener(tNodeGraph, tNodeData, nEventType)
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
    
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData ~= nil then
        local tCltEvents = NodesHandlerMod.getSvrEvents(tNodeData.nNodeType)
        if tCltEvents ~= nil then
            for nEventType, _ in pairs(tCltEvents) do
                delEventListener(tNodeGraph, tNodeData, nEventType)
            end
        end
    end

    nPath = nPath or 1
    local tTransitions = NodeGraphCfgMod.getTransitions(tNodeGraph.tConfigData, sNodeId)
    if tTransitions ~= nil then
        for _, tTransition in ipairs(tTransitions) do
            if tTransition.nPath == nPath then
                CltNodeGraphMod.triggerNode(tNodeGraph, tTransition.sToNodeId)
            end
        end
    end

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
        tNodeGraph.nState = Const.NodeGraphState.Finish
    end
end

function isFinish(tNodeGraph)
    if tNodeGraph == nil then
        return false
    end
    return tNodeGraph.nState == Const.NodeGraphState.Finish
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
        local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
        local nNodeType = tNodeData.nNodeType
        local fEventHandler = NodesHandlerMod.getCltEventHandler(nNodeType, nEventType)
        if fEventHandler ~= nil then
            fEventHandler(tNodeGraph, tNodeData, ...)
        end
    end
end

function recvFinishNode(nGameId, nNodeGraphId, sNodeId, nPath)
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
    CltNodeGraphMod.finishNode(tMainNodeGraph, sNodeId, nPath)
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
