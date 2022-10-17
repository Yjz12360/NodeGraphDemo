local tNodeHandlers = tNodeHandlers or {}

function getCltNodeHandler(nNodeType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    return tHandlers.tCltHandler
end

function getCltEventHandler(nNodeType, nEventType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    local tEventHandlers = tHandlers.tCltEventHandlers
    if tEventHandlers == nil then return end
    return tEventHandlers[nEventType]
end

function getSvrNodeHandler(nNodeType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    return tHandlers.tSvrHandler
end

function getSvrEventHandler(nNodeType, nEventType)
    local tHandlers = tNodeHandlers[nNodeType]
    if tHandlers == nil then return end
    local tEventHandlers = tHandlers.tSvrEventHandlers
    if tEventHandlers == nil then return end
    return tEventHandlers[nEventType]
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

tNodeHandlers[Const.NodeType.AddMonster] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return AddMonsterNode.SvrHandler(tNodeGraph, tNodeData)
    end
}

tNodeHandlers[Const.NodeType.HasMonster] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return HasMonsterNode.SvrHandler(tNodeGraph, tNodeData)
    end
}

tNodeHandlers[Const.NodeType.WaitEnterTrigger] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return WaitEnterTriggerNode.CltHandler(tNodeGraph, tNodeData)
    end,
    tCltEventHandlers = {
        [Const.EventType.EnterTrigger] = function(tNodeGraph, tNodeData, nTriggerId)
            return WaitEnterTriggerNode.CltOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
        end
    },
    tSvrHandler = function(tNodeGraph, tNodeData)
        return WaitEnterTriggerNode.SvrHandler(tNodeGraph, tNodeData)
    end,
    tSvrEventHandlers = {
        [Const.EventType.EnterTrigger] = function(tNodeGraph, tNodeData, nTriggerId)
            return WaitEnterTriggerNode.SvrOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
        end
    },
}
