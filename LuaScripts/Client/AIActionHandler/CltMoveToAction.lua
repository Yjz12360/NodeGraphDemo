
function startAction(tGameObject, tAction)
    local tActionArgs = tAction.tActionArgs
    local goInstance = tGameObject.goInstance
    local uAnimator = tGameObject.uAnimator
    if uAnimator == nil then
        local uMonsterControl = goInstance:GetComponent(typeof(CS.Game.MonsterControl))
        if uMonsterControl ~= nil then
            uAnimator = uMonsterControl.modelAnimator
            tGameObject.uAnimator = uAnimator
        end
    end
    if uAnimator ~= nil then
        uAnimator:SetTrigger("Move")
    end
    local vecPos = goInstance.transform.position
    local nCurrX, nCurrY, nCurrZ = vecPos.x, vecPos.y, vecPos.z
    local nTarX, nTarY, nTarZ = tActionArgs.nPosX, tActionArgs.nPosY, tActionArgs.nPosZ
    local nDirX, nDirY, nDirZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nCurrX, nCurrY, nCurrZ)
    nDirX, nDirY, nDirZ = VectorUtil.normalize(nDirX, nDirY, nDirZ)
    goInstance.transform.forward = UE.Vector3(nDirX, nDirY, nDirZ)
end

function updateAction(tGameObject, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local goInstance = tGameObject.goInstance
    local vecPos = goInstance.transform.position
    local nCurrX, nCurrY, nCurrZ = vecPos.x, vecPos.y, vecPos.z
    local nTarX, nTarY, nTarZ = tActionArgs.nPosX, tActionArgs.nPosY, tActionArgs.nPosZ
    local nDirX, nDirY, nDirZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nCurrX, nCurrY, nCurrZ)
    nDirX, nDirY, nDirZ = VectorUtil.normalize(nDirX, nDirY, nDirZ)
    local nMoveSpeed = tGameObject.tConfig.nMoveSpeed or 1
    local nMoveX, nMoveY, nMoveZ = VectorUtil.mul(nDirX, nDirY, nDirZ, nMoveSpeed * nDeltaTime)
    local nNewX, nNewY, nNewZ = VectorUtil.add(nCurrX, nCurrY, nCurrZ, nMoveX, nMoveY, nMoveZ)
    goInstance.transform.position = UE.Vector3(nNewX, nNewY, nNewZ)
    goInstance.transform.forward = UE.Vector3(nDirX, nDirY, nDirZ)
    local nDiffX, nDiffY, nDiffZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nNewX, nNewY, nNewZ)
    local nDistance = VectorUtil.magnitude(nDiffX, nDiffY, nDiffZ)
    if nDistance <= tActionArgs.nStopDistance then
        CltAIActionMod.finishAction(tAction.nObjectId)
    end
end

function finishAction(tGameObject, tAction)
    local tActionArgs = tAction.tActionArgs
    local goInstance = tGameObject.goInstance
    local uAnimator = tGameObject.uAnimator
    if uAnimator == nil then
        local uMonsterControl = goInstance:GetComponent(typeof(CS.Game.MonsterControl))
        if uMonsterControl ~= nil then
            uAnimator = uMonsterControl.modelAnimator
            tGameObject.uAnimator = uAnimator
        end
    end
    if uAnimator ~= nil then
        uAnimator:SetTrigger("EndMove")
    end
end