
local function doCheck(tNodeGraph, tNodeData)
    local bCheckFlag = true
    local nWaitNode1 = tNodeData.nWaitNode1
    if nWaitNode1 > 0 then
        if not tNodeGraph.tFinishedNodes[nWaitNode1] then
            bCheckFlag = false
        end
    end
    local nWaitNode2 = tNodeData.nWaitNode2
    if nWaitNode2 > 0 then
        if not tNodeGraph.tFinishedNodes[nWaitNode2] then
            bCheckFlag = false
        end
    end
    local nWaitNode3 = tNodeData.nWaitNode3
    if nWaitNode3 > 0 then
        if not tNodeGraph.tFinishedNodes[nWaitNode3] then
            bCheckFlag = false
        end
    end
    return bCheckFlag
end

function SvrHandler(tNodeGraph, tNodeData)
    local bCheckFlag = doCheck(tNodeGraph, tNodeData)
    if bCheckFlag then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeGraph.nNodeId, true)
        return
    end
    local nWaitNode1 = tNodeData.nWaitNode1
    if nWaitNode1 > 0 then
        NodeGraphEventMod.registerNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode1)
    end
    local nWaitNode2 = tNodeData.nWaitNode2
    if nWaitNode2 > 0 then
        NodeGraphEventMod.registerNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode2)
    end
    local nWaitNode3 = tNodeData.nWaitNode3
    if nWaitNode3 > 0 then
        NodeGraphEventMod.registerNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode3)
    end
end

function SvrOnCheck(tNodeGraph, tNodeData)
    local bCheckFlag = doCheck(tNodeGraph, tNodeData)
    if bCheckFlag then
        
        local nWaitNode1 = tNodeData.nWaitNode1
        if nWaitNode1 > 0 then
            NodeGraphEventMod.unregisterNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode1)
        end
        local nWaitNode2 = tNodeData.nWaitNode2
        if nWaitNode2 > 0 then
            NodeGraphEventMod.unregisterNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode2)
        end
        local nWaitNode3 = tNodeData.nWaitNode3
        if nWaitNode3 > 0 then
            NodeGraphEventMod.unregisterNode(tNodeGraph, tNodeGraph.nNodeId, Const.EventType.FinishNode, nWaitNode3)
        end

        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeGraph.nNodeId, true)
    end
end

