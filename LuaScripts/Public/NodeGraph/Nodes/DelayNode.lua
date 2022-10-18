
function CltHandler(tNodeGraph, tNodeData)
    TimerMod.delay(tNodeData.nDelayTime, function()
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end)
end

function SvrHandler(tNodeGraph, tNodeData)
    TimerMod.delay(tNodeData.nDelayTime, function()
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end)
end
