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

-- function getTriggerHandler(nNodeType)
--     local tHandlers = tNodeHandlers[nNodeType]
--     if tHandlers == nil then return end
--     return tHandlers.tTriggerHandler
-- end


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
    tSvrHandler = function(tNodeGraph, tNodeData)
        return WaitEnterTriggerNode.SvrHandler(tNodeGraph, tNodeData)
    end
}

-- tNodeHandlers[Const.NodeType.WaitEnterTrigger] = {
--     -- tCltHandler = function(tNodeGraph, tNodeData)
--     --     return WaitEnterTriggerNode.CltHandler(tNodeGraph, tNodeData)
--     -- end,
--     -- tTriggerHandler = function(tNodeGraph, tNodeData, nTriggerId)
--     --     return WaitEnterTriggerNode.TriggerHandler(tNodeGraph, tNodeData, nTriggerId)
--     -- end,
--     -- tSvrHandler = function(tNodeGraph, tNodeData)
--     --     return WaitEnterTriggerNode.SvrHandler(tNodeGraph, tNodeData)
--     -- end
-- }