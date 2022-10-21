
function SvrOnCheck(tNodeGraph, tNodeData)
    local bCheckFlag = true
    local sWaitNode1 = tNodeData.sWaitNode1
    if sWaitNode1 ~= nil and sWaitNode1 ~= "" then
        if not tNodeGraph.tFinishedNodes[sWaitNode1] then
            bCheckFlag = false
        end
    end
    local sWaitNode2 = tNodeData.sWaitNode2
    if sWaitNode2 ~= nil and sWaitNode2 ~= "" then
        if not tNodeGraph.tFinishedNodes[sWaitNode2] then
            bCheckFlag = false
        end
    end
    local sWaitNode3 = tNodeData.sWaitNode3
    if sWaitNode3 ~= nil and sWaitNode3 ~= "" then
        if not tNodeGraph.tFinishedNodes[sWaitNode3] then
            bCheckFlag = false
        end
    end

    if bCheckFlag then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeGraph.sNodeId)
    end
end

