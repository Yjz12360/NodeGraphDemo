
function startAction(tGameObject, tAction)
    local tActionArgs = tAction.tActionArgs
    TimerMod.add(tActionArgs.nTime, function()
        CltAIActionMod.finishAction(tAction.nObjectId)
    end)
    CltAnimatorMod.setTrigger(tGameObject, "Preform")
end

