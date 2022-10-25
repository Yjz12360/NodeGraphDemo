
function SvrHandler(tNodeGraph, tNodeData)
    local nNodeId = tNodeData.nNodeId
    local nTriggerId = tNodeData.nTriggerId
    NodeGraphEventMod.registerNode(tNodeGraph, nNodeId, Const.EventType.EnterTrigger, nTriggerId)
end

function SvrOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
    if nTriggerId == tNodeData.nTriggerId then
        local nNodeId = tNodeData.nNodeId
        NodeGraphEventMod.unregisterNode(tNodeGraph, nNodeId, Const.EventType.EnterTrigger, nTriggerId)
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
    end
end
