

function CltOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
    if nTriggerId == tNodeData.nTriggerId then
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

function SvrOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
    if nTriggerId == tNodeData.nTriggerId then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end
