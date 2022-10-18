
function startAction(tAI, tAction)
    local tActionArgs = tAction.tActionArgs
    local nTarX, nTarY, nTarZ = tActionArgs.nPosX, tActionArgs.nPosY, tActionArgs.nPosZ
    local nStopDistance = tActionArgs.nStopDistance
    Messager.S2CAIMoveTo(tAI.nGameId, tAI.nObjectId, nTarX, nTarY, nTarZ, nStopDistance)
end

function updateAction(tAI, tAction, nDeltaTime)
    local tActionArgs = tAction.tActionArgs
    local tSvrGame = SvrGameMod.getGameById(tAI.nGameId)

    local tObject = SvrGameMod.getObject(tSvrGame, tAI.nObjectId)
    local nCurrX, nCurrY, nCurrZ = tObject.nPosX, tObject.nPosY, tObject.nPosZ
    local nTarX, nTarY, nTarZ = tActionArgs.nPosX, tActionArgs.nPosY, tActionArgs.nPosZ
    local nOffsetX, nOffsetY, nOffsetZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nCurrX, nCurrY, nCurrZ)
    local nDirX, nDirY, nDirZ = VectorUtil.normalize(nOffsetX, nOffsetY, nOffsetZ)
    local nSpeed = tObject.tConfig.nMoveSpeed or 1
    local nMoveX, nMoveY, nMoveZ = VectorUtil.mul(nDirX, nDirY, nDirZ, nSpeed * nDeltaTime)

    local nNewX, nNewY, nNewZ = nCurrX + nMoveX, nCurrY + nMoveY, nCurrZ + nMoveZ

    tObject.nPosX, tObject.nPosY, tObject.nPosZ = nNewX, nNewY, nNewZ

    local nDiffX, nDiffY, nDiffZ = VectorUtil.sub(nTarX, nTarY, nTarZ, nNewX, nNewY, nNewZ)
    local nDistance = VectorUtil.magnitude(nDiffX, nDiffY, nDiffZ)
    if nDistance <= tActionArgs.nStopDistance then
        AIMod.finishAction(tAI)
    end
end