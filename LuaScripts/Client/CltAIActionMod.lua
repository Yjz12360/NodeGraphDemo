
local tActions = {}
local tRegUpdateFunc = {}

local tActionHandler = {}
tActionHandler[Const.AIActionType.Idle] = {
    fStartAction = function(tGameObject, tAction)
        return CltIdleAction.startAction(tGameObject, tAction)
    end,
    fUpdateAction = function(tGameObject, tAction, nDeltaTime)
        return CltIdleAction.updateAction(tGameObject, tAction, nDeltaTime)
    end,
    fFinishAction = function(tGameObject, tAction)
        return CltIdleAction.finishAction(tGameObject, tAction)
    end
}

tActionHandler[Const.AIActionType.MoveTo] = {
    fStartAction = function(tGameObject, tAction)
        return CltMoveToAction.startAction(tGameObject, tAction)
    end,
    fUpdateAction = function(tGameObject, tAction, nDeltaTime)
        return CltMoveToAction.updateAction(tGameObject, tAction, nDeltaTime)
    end,
    fFinishAction = function(tGameObject, tAction)
        return CltMoveToAction.finishAction(tGameObject, tAction)
    end
}


local function onAddAction(nGameId, nActionType, nObjectId, tActionArgs)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end

    local tAction = {
        nObjectId = nObjectId,
        nActionType = nActionType,
        tActionArgs = tActionArgs,
    }
    tActions[nObjectId] = tAction
    local tHandlers = tActionHandler[nActionType]
    if tHandlers == nil then
        return
    end
    local fStartAction = tHandlers.fStartAction
    if fStartAction ~= nil then
        local tGameObject = CltGameMod.getObject(tAction.nObjectId)
        if tGameObject ~= nil then
            fStartAction(tGameObject, tAction)
        end
    end
    local fUpdateAction = tHandlers.fUpdateAction
    if fUpdateAction ~= nil then
        tRegUpdateFunc[nObjectId] = true
    end
end

function onAIMoveTo(nGameId, nObjectId, nPosX, nPosY, nPosZ, nStopDistance)
    onAddAction(nGameId, Const.AIActionType.MoveTo, nObjectId, {
        nPosX = nPosX,
        nPosY = nPosY,
        nPosZ = nPosZ,
        nStopDistance = nStopDistance,
    })
end

-- TODO 没有action时不调用update
tToDel = nil
function update(nDeltaTime)
    for nObjectId, _ in pairs(tRegUpdateFunc) do
        local tAction = tActions[nObjectId]
        if tAction ~= nil then
            local tGameObject = CltGameMod.getObject(tAction.nObjectId)
            if tGameObject ~= nil then
                local tHandlers = tActionHandler[tAction.nActionType]
                local fUpdateAction = tHandlers.fUpdateAction
                if fUpdateAction ~= nil then
                    fUpdateAction(tGameObject, tAction, nDeltaTime)
                end
            else
                tToDel = tToDel or {}
                table.insert(tToDel, nObjectId)
            end
        end
    end
    if tToDel ~= nil then
        for _, nObjectId in ipairs(tToDel) do
            tRegUpdateFunc[nObjectId] = nil
        end
        tToDel = nil
    end
end

function finishAction(nObjectId)
    local tAction = tActions[nObjectId]
    if tAction == nil then
        return 
    end
    local tHandler = tActionHandler[tAction.nActionType]
    if tHandler ~= nil then
        local fFinishAction = tHandler.fFinishAction
        if fFinishAction ~= nil then
            local tGameObject = CltGameMod.getObject(nObjectId)
            if tGameObject ~= nil then
                fFinishAction(tGameObject, tAction)
            end
        end
    end
    tRegUpdateFunc[nObjectId] = nil
    tActions[nObjectId] = nil
end

