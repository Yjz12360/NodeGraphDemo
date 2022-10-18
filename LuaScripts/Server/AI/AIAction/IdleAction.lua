
function startAction(tMonsterAI, tAction)
    tAction.tActionData.nCurrTime = 0
end

function updateAction(tMonsterAI, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    tActionData.nCurrTime = tActionData.nCurrTime + nDeltaTime
    local nTime = tActionArgs and tActionArgs.nTime or 0
    if tActionData.nCurrTime >= nTime then
        AIMod.finishAction(tMonsterAI)
    end
end