
function CltHandler(tNodeGraph, tNodeData)
    function fTriggerEnter(nTriggerId)
        if nTriggerId == tNodeData.nTriggerId then
            CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
    return {
        fTriggerEnter = fTriggerEnter,
    }
end

function SvrHandler(tNodeGraph, tNodeData)
    function fTriggerEnter(nTriggerId)
        if nTriggerId == tNodeData.nTriggerId then
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
    return {
        fTriggerEnter = fTriggerEnter,
    }
end

-- function TriggerHandler(tNodeGraph, tNodeData, nTriggerId)
--     if nTriggerId == tNodeData.nTriggerId then
--         CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
--     end
-- end

-- function SvrHandler(tNodeGraph, tNodeData)

-- end