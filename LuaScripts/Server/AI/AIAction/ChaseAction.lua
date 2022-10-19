
function startAction(tAI, tAction)
    tAction.tActionData.nChaseTimer = 0
    local tActionArgs = tAction.tActionArgs
    local nChaseTime = tActionArgs.nChaseTime
    local nStopDistance = tActionArgs.nStopDistance
    Messager.S2CAIChase(tAI.nGameId, tAI.nObjectId, tAI.nTargetId, nChaseTime, nStopDistance)
end

function updateAction(tAI, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    local nTargetId = tActionArgs.nTargetId
    local nChaseTime = tActionArgs.nChaseTime
    local nStopDistance = tActionArgs.nStopDistance

    tActionData.nChaseTimer = tActionData.nChaseTimer + nDeltaTime
    if tActionData.nChaseTimer >= nChaseTime then
        AIMod.finishAction(tAI)
        return
    end

    local tTargetObject = SvrGameMod.getObject(nTargetId)
    if tTargetObject == nil then
        AIMod.finishAction(tAI)
        return
    end
    local tGameObject = SvrGameMod.getObject(tAI.nObjectId)
    
    local nTarX, nTarY, nTarZ = tTargetObject.nPosX, tTargetObject.nPosY, tTargetObject.nPosZ
    local nCurrX, nCurrY, nCurrZ = tGameObject.nPosX, tGameObject.nPosY, tGameObject.nPosZ
    local nDirX, nDirY, nDirZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nCurrX, nCurrY, nCurrZ) 
    nDirX, nDirY, nDirZ = VectorUtil.normalize(nDirX, nDirY, nDirZ)
    local nMoveSpeed = tGameObject.tConfig.nMoveSpeed or 1
    local nMoveX, nMoveY, nMoveZ = VectorUtil.mul(nDirX, nDirY, nDirZ, nMoveSpeed * nDeltaTime)
    local nNewX, nNewY, nNewZ = VectorUtil.add(nCurrX, nCurrY, nCurrZ, nMoveX, nMoveY, nMoveZ)
    tGameObject.nPosX, tGameObject.nPosY, tGameObject.nPosZ = nNewX, nNewY, nNewZ
    local nDiffX, nDiffY, nDiffZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nNewX, nNewY, nNewZ)
    local nSqrDistance = VectorUtil.sqrMagnitude(nDiffX, nDiffY, nDiffZ)
    if nSqrDistance <= tActionArgs.nStopDistance ^ 2 then
        AIMod.finishAction(tAI)
    end
end