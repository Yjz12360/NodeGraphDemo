local tNodeHandlers = tNodeHandlers or {}
local tCltEvents = {}
local tSvrEvents = {}

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

function getCltEvents(nNodeType)
    return tCltEvents[nNodeType]
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

function getSvrEvents(nNodeType)
    return tSvrEvents[nNodeType]
end

function init()
    for _, nNodeType in pairs(Const.NodeType) do
        local tHandlers = tNodeHandlers[nNodeType]
        if tHandlers ~= nil then
            local tCltEventHandlers = tHandlers.tCltEventHandlers
            if tCltEventHandlers ~= nil then
                for nEventType, _ in pairs(tCltEventHandlers) do
                    local tEvents = tCltEvents[nNodeType]
                    if tEvents == nil then
                        tEvents = {}
                        tCltEvents[nNodeType] = tEvents
                    end
                    tEvents[nEventType] = true
                end
            end
            local tSvrEventHandlers = tHandlers.tSvrEventHandlers
            if tSvrEventHandlers ~= nil then
                for nEventType, _ in pairs(tSvrEventHandlers) do
                    local tEvents = tSvrEvents[nNodeType]
                    if tEvents == nil then
                        tEvents = {}
                        tSvrEvents[nNodeType] = tEvents
                    end
                    tEvents[nEventType] = true
                end
            end
        end
    end
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
    tCltEventHandlers = {
        [Const.EventType.EnterTrigger] = function(tNodeGraph, tNodeData, nTriggerId)
            return WaitEnterTriggerNode.CltOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
        end
    },
    tSvrEventHandlers = {
        [Const.EventType.EnterTrigger] = function(tNodeGraph, tNodeData, nTriggerId)
            return WaitEnterTriggerNode.SvrOnTriggerEnter(tNodeGraph, tNodeData, nTriggerId)
        end
    },
}

tNodeHandlers[Const.NodeType.WaitMonsterNum] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return WaitMonsterNumNode.SvrOnCheck(tNodeGraph, tNodeData)
    end,
    tSvrEventHandlers = {
        [Const.EventType.AddMonster] = function(tNodeGraph, tNodeData)
            return WaitMonsterNumNode.SvrOnCheck(tNodeGraph, tNodeData)
        end,
        [Const.EventType.MonsterDead] = function(tNodeGraph, tNodeData)
            return WaitMonsterNumNode.SvrOnCheck(tNodeGraph, tNodeData)
        end
    },
}

tNodeHandlers[Const.NodeType.WaitMonsterDead] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return WaitMonsterDeadNode.SvrHandler(tNodeGraph, tNodeData)
    end,
    tSvrEventHandlers = {
        [Const.EventType.BeforeMonsterDead] = function(tNodeGraph, tNodeData, nObjectId)
            return WaitMonsterDeadNode.SvrBeforeMonsterDead(tNodeGraph, tNodeData, nObjectId)
        end
    },
}

tNodeHandlers[Const.NodeType.AnimatorCtrl] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return AnimatorCtrlNode.CltHandler(tNodeGraph, tNodeData)
    end,
}

tNodeHandlers[Const.NodeType.ActiveAI] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return ActiveAINode.SvrHandler(tNodeGraph, tNodeData)
    end,
}

tNodeHandlers[Const.NodeType.RefreshMonsterGroup] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return RefreshMonsterGroupNode.SvrHandler(tNodeGraph, tNodeData)
    end,
}

tNodeHandlers[Const.NodeType.Random] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return RandomNode.SvrHandler(tNodeGraph, tNodeData)
    end,
}

tNodeHandlers[Const.NodeType.WaitAllNodeFinish] = {
    tSvrHandler = function(tNodeGraph, tNodeData)
        return WaitAllNodeFinishNode.SvrOnCheck(tNodeGraph, tNodeData)
    end,
    tSvrEventHandlers = {
        [Const.EventType.FinishNode] = function(tNodeGraph, tNodeData, sNodeId)
            return WaitAllNodeFinishNode.SvrOnCheck(tNodeGraph, tNodeData)
        end
    },
}

tNodeHandlers[Const.NodeType.SetPosition] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return SetPositionNode.CltHandler(tNodeGraph, tNodeData)
    end,
    tSvrHandler = function(tNodeGraph, tNodeData)
        return SetPositionNode.SvrHandler(tNodeGraph, tNodeData)
    end,
}

tNodeHandlers[Const.NodeType.CameraTrace] = {
    tCltHandler = function(tNodeGraph, tNodeData)
        return CameraTraceNode.CltHandler(tNodeGraph, tNodeData)
    end,
    tSvrHandler = function(tNodeGraph, tNodeData)
        return CameraTraceNode.SvrHandler(tNodeGraph, tNodeData)
    end,
}

init()