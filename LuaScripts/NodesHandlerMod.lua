local tNodeHandlers = tNodeHandlers or {}

function getCltNodeHandler(nNodeType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    return tHandlers.tCltHandler
end

function getSvrNodeHandler(nNodeType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    return tHandlers.tSvrHandler
end

tNodeHandlers[Const.NodeType.Print] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return PrintNode.CltHandler(tNodeGraph, tNodeData)
    end
}

tNodeHandlers[Const.NodeType.Delay] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return DelayNode.CltHandler(tNodeGraph, tNodeData)
    end,
    tSvrHandler = function(tNodeGraph, tNodeData)
        return DelayNode.SvrHandler(tNodeGraph, tNodeData)
    end
}

