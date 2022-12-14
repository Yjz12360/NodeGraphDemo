
function startAction(tGameObject, tAction)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    CltAnimatorMod.startMove(tGameObject)
    tActionData.nChaseTimer = 0
end

function updateAction(tGameObject, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local tActionData = tAction.tActionData
    tActionData.nChaseTimer = tActionData.nChaseTimer + nDeltaTime
    if tActionData.nChaseTimer >= nChaseTime then
        CltAIActionMod.finishAction(tAction.nObjectId)
        return
    end

    local tTargetObject = CltGameMod.getObject(nTargetId)
    if tTargetObject == nil then
        CltAIActionMod.finishAction(tAction.nObjectId)
        return
    end

    local goInstance = tGameObject.goInstance
    local vecPos = goInstance.transform.position
    local nCurrX, nCurrY, nCurrZ = vecPos.x, vecPos.y, vecPos.z
    local nTarX, nTarY, nTarZ = tTargetObject.nPosX, tTargetObject.nPosY, tTargetObject.nPosZ
    local nDirX, nDirY, nDirZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nCurrX, nCurrY, nCurrZ)
    nDirX, nDirY, nDirZ = VectorUtil.normalize(nDirX, nDirY, nDirZ)
    local nMoveSpeed = tGameObject.tConfig.nMoveSpeed or 1
    local nMoveX, nMoveY, nMoveZ = VectorUtil.mul(nDirX, nDirY, nDirZ, nMoveSpeed * nDeltaTime)
    CltCharacterControllerMod.move(tGameObject, nMoveX, nMoveY, nMoveZ)
    goInstance.transform.forward = UE.Vector3(nDirX, nDirY, nDirZ)
    local nNewX, nNewY, nNewZ = VectorUtil.add(nCurrX, nCurrY, nCurrZ, nMoveX, nMoveY, nMoveZ)
    local nDiffX, nDiffY, nDiffZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nNewX, nNewY, nNewZ)
    local nDistance = VectorUtil.magnitude(nDiffX, nDiffY, nDiffZ)
    if nDistance <= tActionArgs.nStopDistance then
        CltAIActionMod.finishAction(tAction.nObjectId)
    end
end

function finishAction(tGameObject, tAction)
    CltAnimatorMod.endMove(tGameObject)
end