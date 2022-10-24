
function onEnterTrigger(nGameId, nTriggerId)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then
        return
    end
    SvrGameMod.processEvent(tSvrGame, Const.EventType.EnterTrigger, nTriggerId)
end

function onAttackHit(nGameId, nAttackerId, nTargetId)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then
        return
    end
    local tAttacker = SvrGameMod.getObject(tSvrGame, nAttackerId)
    if tAttacker == nil then
        return
    end
    local tTarget = SvrGameMod.getObject(tSvrGame, tTarget)
    if tTarget == nil then
        return 
    end
    local nAtk = tAttacker.tConfig.nAtk
    tTarget.nCurrHP = tTarget.nCurrHP - nAtk
    if tTarget.nCurrHP <= 0 then
        tTarget.nCurrHP = 0
        SvrGameRoleMod.roleDead(tSvrGame, nTargetId)
    end
end

function onSyncLocalPlayerPos(nGameId, nObjectId, nPosX, nPosY, nPosZ)
    local tSvrGame = SvrGameMod.getGameById(nGameId)
    if tSvrGame == nil then
        return
    end
    local tPlayer = SvrGameMod.getObject(tSvrGame, nObjectId)
    if tPlayer == nil or tPlayer.nObjectType ~= Const.GameObjectType.Player then
        return
    end
    tPlayer.nPosX = nPosX
    tPlayer.nPosY = nPosY
    tPlayer.nPosZ = nPosZ
    -- TODO 广播其他玩家
end