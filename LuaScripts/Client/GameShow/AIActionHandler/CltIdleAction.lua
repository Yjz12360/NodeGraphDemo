
function startAction(tGameObject, tAction)
    local tActionArgs = tAction.tActionArgs
    TimerMod.delay(tActionArgs.nTime, function()
        CltAIActionMod.finishAction(tAction.nObjectId)
    end)
    -- CltAnimatorMod.setTrigger(tGameObject, "Preform")
end

