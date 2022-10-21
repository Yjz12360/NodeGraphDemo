
local tLocalPlayer

local nSyncPosTimer
local nSyncPosInv = 0.2
local bLocalMove = false
local function handleSyncPos()
    if nSyncPosTimer ~= nil then
        TimerMod.remove(nSyncPosTimer)
    end
    nSyncPosTimer = TimerMod.add(nSyncPosInv, function()
        if tLocalPlayer == nil then
            TimerMod.remove(nSyncPosTimer)
            return
        end
        if bLocalMove then
            bLocalMove = false
            local nGameId = CltGameMod.getGameId()
            local nObjectId = tLocalPlayer.nObjectId
            local goInstance = tLocalPlayer.goInstance
            if goInstance ~= nil then
                local vecPos = goInstance.transform.position
                Messager.C2SSyncLocalPlayerPos(nGameId, nObjectId, vecPos.x, vecPos.y, vecPos.z)
            end
        end
    end)
end

function setLocalPlayer(tPlayer)
    tLocalPlayer = tPlayer

    if tLocalPlayer ~= nil then
        handleSyncPos()
    end
end

local function applyMove(nDirX, nDirZ, nDeltaTime)
    if tLocalPlayer == nil then
        return
    end
    local goInstance = tLocalPlayer.goInstance
    if goInstance == nil then
        return
    end
    local nMoveSpeed = tLocalPlayer.tConfig.nMoveSpeed
    local nMoveDistance = nMoveSpeed * nDeltaTime
    local vecPos = goInstance.transform.position

    local uCharactorController = tLocalPlayer.uCharactorController
    if uCharactorController == nil then
        uCharactorController = goInstance:GetComponent(typeof(UE.CharacterController))
        tLocalPlayer.uCharactorController = uCharactorController
    end
    local nMoveX = nDirX * nMoveDistance
    local nMoveZ = nDirZ * nMoveDistance
    CltCharacterControllerMod.move(tLocalPlayer, nMoveX, 0, nMoveZ)
    goInstance.transform.forward = UE.Vector3(nDirX, 0, nDirZ)
    if not tLocalPlayer.bLastMoving then
        CltAnimatorMod.startMove(tLocalPlayer)
    end
    tLocalPlayer.bLastMoving = true
    bLocalMove = true
end

local function idle()
    if tLocalPlayer == nil then
        return
    end
    if tLocalPlayer.bLastMoving then
        tLocalPlayer.bLastMoving = false
        CltAnimatorMod.endMove(tLocalPlayer)
    end
end

local function launchAttack()
    if tLocalPlayer == nil then
        return
    end
    local goInstance = tLocalPlayer.goInstance
    if goInstance == nil then
        return
    end
    if tLocalPlayer.bAttacking then
        return
    end
    tLocalPlayer.bAttacking = true
    local uAttackCollider = tLocalPlayer.uAttackCollider
    if uAttackCollider == nil then
        uAttackCollider = goInstance:GetComponentInChildren(typeof(CS.PlayerAttack))
        tLocalPlayer.uAttackCollider = uAttackCollider
    end
    TimerMod.delay(0.5, function()
        uAttackCollider.gameObject.active = true
    end)
    TimerMod.delay(1, function()
        uAttackCollider.gameObject.active = false
        tLocalPlayer.bAttacking = false
    end)
end

local GetKey = CS.UnityEngine.Input.GetKey
local GetKeyDown = CS.UnityEngine.Input.GetKeyDown
local KeyCode = CS.UnityEngine.KeyCode
function update(nDeltaTime) -- TODO 能否改成事件驱动输入
    if GetKey(KeyCode.W) then
        applyMove(0, 1, nDeltaTime)
    elseif GetKey(KeyCode.S) then
        applyMove(0, -1, nDeltaTime)
    elseif GetKey(KeyCode.A) then
        applyMove(-1, 0, nDeltaTime)
    elseif GetKey(KeyCode.D) then
        applyMove(1, 0, nDeltaTime)
    else
        idle()
    end
    if GetKeyDown(KeyCode.Space) then
        launchAttack()
    end
end
