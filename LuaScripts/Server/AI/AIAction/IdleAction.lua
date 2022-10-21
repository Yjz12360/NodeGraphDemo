
function startAction(tAI, tAction)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    tActionData.nCurrTime = 0
    Messager.S2CAIIdle(tAI.nGameId, tAI.nObjectId, tActionArgs.nTime)
end

function updateAction(tAI, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    tActionData.nCurrTime = tActionData.nCurrTime + nDeltaTime
    local nTime = tActionArgs and tActionArgs.nTime or 0
    if tActionData.nCurrTime >= nTime then
        AIMod.finishAction(tAI)
    end
end