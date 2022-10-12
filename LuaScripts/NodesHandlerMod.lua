PrintNode = require "Nodes/PrintNode"
DelayNode = require "Nodes/DelayNode"

NodeType = {
    Start = 0,
    Print = 1,
    Delay = 3,
}

local tNodeHandlers = tNodeHandlers or {}

tNodeHandlers[NodeType.Print] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return PrintNode.CltHandler(tNodeGraph, tNodeData)
    end
}

tNodeHandlers[NodeType.Delay] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return DelayNode.CltHandler(tNodeGraph, tNodeData)
    end,
    tSvrHandler = function(tNodeGraph, tNodeData)
        return DelayNode.SvrHandler(tNodeGraph, tNodeData)
    end
}

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

return {
    getCltNodeHandler = getCltNodeHandler,
    getSvrNodeHandler = getSvrNodeHandler,
}