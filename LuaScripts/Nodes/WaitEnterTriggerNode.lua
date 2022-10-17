
function CltHandler(tNodeGraph, tNodeData)
    CltNodeGraphMod.addEventListener(tNodeGraph, tNodeData, Const.EventType.EnterTrigger)
end

function CltOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
    if nTriggerId == tNodeData.nTriggerId then
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

function SvrHandler(tNodeGraph, tNodeData)
    SvrNodeGraphMod.addEventListener(tNodeGraph, tNodeData, Const.EventType.EnterTrigger)
end

function SvrOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
    if nTriggerId == tNodeData.nTriggerId then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end
