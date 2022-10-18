
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

tActionHandlers[AIConst.ActionType.Idle] = {
    fStart = function(tMonsterAI, tAction)
        return IdleAction.startAction(tMonsterAI, tAction)
    end,
    fUpdate = function(tMonsterAI, tAction, nDeltaTime)
        return IdleAction.updateAction(tMonsterAI, tAction, nDeltaTime)
    end
}