
function addAI(nGameId, nObjectId)
    local tAI = {}
    tAI.nGameId = nGameId
    tAI.nObjectId = nObjectId
    tAI.tCurrAction = nil
    tAI.tLastAction = nil
    tAI.bActive = true
    return tAI
end

function getGame(tAI)
    return SvrGameMod.getGameById(tAI.nGameId)
end

function addAction(tAI, nActionType, tActionArgs)
    if tAI == nil then
        return
    end
    if tAI.tCurrAction ~= nil then
        printError("AIMod addAction error: action already exists.")
        return
    end
    local tAction = {
        nActionType = nActionType,
        tActionArgs = tActionArgs,
        tActionData = {},
    }
    tAI.tCurrAction = tAction
    local fStart = AIActionHandlerMod.getStartHandler(nActionType)
    if fStart ~= nil then
        fStart(tAI, tAction)
    end
end

-- TODO 支持多种样式AI;仅在有怪物时更新
function update(tAI, nDeltaTime)
    if not tAI.bActive then
        return
    end
    local tCurrAction = tAI.tCurrAction
    if tCurrAction == nil then
        local tLastAction = tAI.tLastAction
        if tLastAction == nil or tLastAction.nActionType == Const.AIActionType.MoveTo then
            local nIdleTime = 0.5 + math.random() * 2
            AIMod.addAction(tAI, Const.AIActionType.Idle, {nTime = nIdleTime})
        else
            local nRad = math.random() * 2 * math.pi
            local nDirX, nDirY, nDirZ = VectorUtil.rad2Vec(nRad)
            local nDistance = 2 + math.random() * 3
            local nMoveX, nMoveY, nMoveZ = VectorUtil.mul(nDirX, nDirY, nDirZ, nDistance)
            local tSvrGame = SvrGameMod.getGameById(tAI.nGameId)
            local tGameObject = SvrGameMod.getObject(tSvrGame, tAI.nObjectId)
            local nCurrX, nCurrY, nCurrZ = tGameObject.nPosX, tGameObject.nPosY, tGameObject.nPosZ
            local nTarX, nTarY, nTarZ = VectorUtil.add(nCurrX, nCurrY, nCurrZ, nMoveX, nMoveY, nMoveZ)
            AIMod.addAction(tAI, Const.AIActionType.MoveTo, {
                nPosX = nTarX,
                nPosY = nTarY,
                nPosZ = nTarZ,
                nStopDistance = 1,
            })
        end
        -- AIMod.addAction(tAI, Const.AIActionType.Idle, {nTime = 1.0})
    else
        local fUpdate = AIActionHandlerMod.getUpdateHandler(tCurrAction.nActionType)
        if fUpdate ~= nil then
            fUpdate(tAI, tCurrAction, nDeltaTime)
        end
    end
end

function finishAction(tAI)
    tAI.tLastAction = tAI.tCurrAction
    tAI.tCurrAction = nil
end

function setAIActive(tAI, bActive)
    if tAI == nil then
        return
    end
    tAI.bActive = bActive
    if not bActive then
        AIMod.finishAction(tAI)
    end
end