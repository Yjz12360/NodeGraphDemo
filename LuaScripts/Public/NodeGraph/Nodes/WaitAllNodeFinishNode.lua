
local function doCheck(tNodeGraph, tNodeData)
    local bCheckFlag = true

    local tWaitNodes = tNodeData.tWaitNodes
    for _, nWaitNodeId in ipairs(tWaitNodes) do
        if nWaitNodeId > 0 then
            if not tNodeGraph.tFinishedNodes[nWaitNodeId] then
                bCheckFlag = false
                break
            end
        end
    end
    return bCheckFlag
end

function SvrHandler(tNodeGraph, tNodeData)
    local nNodeId = tNodeData.nNodeId
    local bCheckFlag = doCheck(tNodeGraph, tNodeData)
    if bCheckFlag then
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId, true)
        return
    end
    local tWaitNodes = tNodeData.tWaitNodes
    for _, nWaitNodeId in ipairs(tWaitNodes) do
        if nWaitNodeId > 0 then
            NodeGraphEventMod.registerNode(tNodeGraph, nNodeId, Const.EventType.FinishNode, nWaitNodeId)
        end
    end
end

function SvrOnCheck(tNodeGraph, tNodeData)
    local bCheckFlag = doCheck(tNodeGraph, tNodeData)
    if bCheckFlag then
        local nNodeId = tNodeData.nNodeId
        local tWaitNodes = tNodeData.tWaitNodes
        for _, nWaitNodeId in ipairs(tWaitNodes) do
            if nWaitNodeId > 0 then
                NodeGraphEventMod.unregisterNode(tNodeGraph, nNodeId, Const.EventType.FinishNode, nWaitNodeId)
            end
        end
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId, true)
    end
end

