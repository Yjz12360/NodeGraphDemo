
function CltHandler(tNodeGraph, tNodeData)
    local nTimer = 0
    function fUpdate(nDeltaTime)
        nTimer = nTimer + nDeltaTime
        if nTimer >= tNodeData.nDelayTime then
            CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
    return {
        fUpdate = fUpdate,
    }
end

function SvrHandler(tNodeGraph, tNodeData)
    local nTimer = 0
    function fUpdate(nDeltaTime)
        nTimer = nTimer + nDeltaTime
        if nTimer >= tNodeData.nDelayTime then
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
    return {
        fUpdate = fUpdate,
    }
end
