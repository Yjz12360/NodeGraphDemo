
local tActionHandlers = tActionHandlers or {}

function getStartHandler(nActionType)
    if tActionHandlers[nActionType] == nil then
        return nil
    end
    return tActionHandlers[nActionType].fStart
end

function getUpdateHandler(nActionType)
    if tActionHandlers[nActionType] == nil then
        return nil
    end
    return tActionHandlers[nActionType].fUpdate
end

tActionHandlers[Const.AIActionType.Idle] = {
    fStart = function(tAI, tAction)
        return IdleAction.startAction(tAI, tAction)
    end,
    fUpdate = function(tAI, tAction, nDeltaTime)
        return IdleAction.updateAction(tAI, tAction, nDeltaTime)
    end
}

tActionHandlers[Const.AIActionType.MoveTo] = {
    fStart = function(tAI, tAction)
        return MoveToAction.startAction(tAI, tAction)
    end,
    fUpdate = function(tAI, tAction, nDeltaTime)
        return MoveToAction.updateAction(tAI, tAction, nDeltaTime)
    end
}