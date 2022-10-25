
function init(tNodeGraph)
    if tNodeGraph == nil then
        return
    end
    tNodeGraph.tEventNodeMap = {}
    tNodeGraph.tIdEventNodeMap = {}
end

function registerNode(tNodeGraph, nNodeId, nEventType, nEventLocalId)
    if tNodeGraph == nil then
        return
    end
    if nEventLocalId ~= nil then
        local tIdEventNodeMap = tNodeGraph.tIdEventNodeMap
        local tIdEventNodes = tIdEventNodeMap[nEventType]
        if tIdEventNodes == nil then
            tIdEventNodes = {}
            tIdEventNodeMap[nEventType] = tIdEventNodes
        end
        local tEventNodes = tIdEventNodes[nEventLocalId]
        if tEventNodes == nil then
            tEventNodes = {}
            tIdEventNodes[nEventLocalId] = tEventNodes
        end
        tEventNodes[nNodeId] = true
    else
        local tEventNodeMap = tNodeGraph.tEventNodeMap
        local tEventNodes = tEventNodeMap[nEventType]
        if tEventNodes == nil then
            tEventNodes = {}
            tNodeEventMap[nEventType] = tEventNodes
        end
        tEventNodes[nNodeId] = true
    end
end

function unregisterNode(tNodeGraph, nNodeId, nEventType, nEventLocalId)
    if tNodeGraph == nil then
        return
    end
    if nEventLocalId ~= nil then
        local tIdEventNodeMap = tNodeGraph.tIdEventNodeMap
        local tIdEventNodes = tIdEventNodeMap[nEventType]
        if tIdEventNodes == nil then
            return
        end
        local tEventNodes = tIdEventNodes[nEventLocalId]
        if tEventNodes == nil then
            return
        end
        tEventNodes[nNodeId] = nil
        if TableUtil.isEmpty(tEventNodes) then
            tIdEventNodes[nEventLocalId] = nil
            if TableUtil.isEmpty(tIdEventNodes) then
                tIdEventNodeMap[nEventType] = nil
            end
        end
    else
        local tEventNodeMap = tNodeGraph.tEventNodeMap
        local tEventNodes = tEventNodeMap[nEventType]
        if tEventNodes == nil then
            return
        end
        tEventNodes[nNodeId] = nil
        if TableUtil.isEmpty(tEventNodes) then
            tEventNodeMap[nEventType] = nil
        end
    end
end

local function handleEvent(tNodeGraph, nNodeId, nEventType, ...)
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, nNodeId)
    local nNodeType = tNodeData.nNodeType
    local fEventHandler
    if tNodeGraph.bServer then
        fEventHandler = NodesHandlerMod.getSvrEventHandler(nNodeType, nEventType)
    else
        fEventHandler = NodesHandlerMod.getCltEventHandler(nNodeType, nEventType)
    end
    if fEventHandler ~= nil then
        fEventHandler(tNodeGraph, tNodeData, ...)
    end
end

function processEvent(tNodeGraph, nEventType, nEventLocalId, ...)
    if tNodeGraph == nil then
        return
    end
    local tEventNodeMap = tNodeGraph.tEventNodeMap
    local tEventNodes = tEventNodeMap[nEventType]
    if tEventNodes ~= nil then
        for nNodeId, _ in pairs(tEventNodes) do
            handleEvent(tNodeGraph, nNodeId, nEventType, nEventLocalId, ...)
        end
    end
    local tIdEventNodeMap = tNodeGraph.tIdEventNodeMap
    local tIdEventNodes = tIdEventNodeMap[nEventType]
    if tIdEventNodes ~= nil then
        local tEventNodes = tIdEventNodes[nEventLocalId]
        if tEventNodes ~= nil then
            for nNodeId, _ in pairs(tEventNodes) do
                handleEvent(tNodeGraph, nNodeId, nEventType, nEventLocalId, ...)
            end
        end
    end
end
