
function SvrHandler(tNodeGraph, tNodeData)
    local nPath
    if math.random() > 0.5 then
        nPath = 1
    else
        nPath = 2
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, nPath)
end
