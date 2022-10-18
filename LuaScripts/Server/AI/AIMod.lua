
function addAI(nGameId, nObjectId)
    local tAI = {}
    tAI.nGameId = nGameId
    tAI.nObjectId = nObjectId
    tAI.tCurrAction = nil
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

function update(tAI, nDeltaTime)
    local tCurrAction = tAI.tCurrAction
    if tCurrAction == nil then
        AIMod.addAction(tAI, AIConst.ActionType.Idle, {nTime = 1.0})
    else
        local fUpdate = AIActionHandlerMod.getUpdateHandler(tCurrAction.nActionType)
        if fUpdate ~= nil then
            fUpdate(tAI, tCurrAction, nDeltaTime)
        end
    end
end

function finishAction(tAI)
    printError("finishAction")
    tAI.tCurrAction = nil
end