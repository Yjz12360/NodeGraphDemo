
local tFallingController = nil
local nFallSpeed = 9.8 * 5

local function getController(tGameObject)
    if tGameObject == nil then
        return
    end
    local uCharactorController = tGameObject.uCharactorController
    if uCharactorController == nil then
        local goInstance = tGameObject.goInstance
        uCharactorController = goInstance:GetComponent(typeof(UE.CharacterController))
        tGameObject.uCharactorController = uCharactorController
    end
    return uCharactorController
end

function move(tGameObject, nMoveX, nMoveY, nMoveZ)
    local uCharactorController = getController(tGameObject)
    if uCharactorController == nil then
        return
    end
    local vecMove = UE.Vector3(nMoveX, nMoveY, nMoveZ)
    uCharactorController:Move(vecMove)
    if not uCharactorController.isGrounded then
        tFallingController = tFallingController or {}
        tFallingController[tGameObject.nObjectId] = uCharactorController
    end
end

local tFalledControllers = nil
function update(nDeltaTime)
    if tFallingController ~= nil then
        local vecDown = UE.Vector3(0, -nFallSpeed * nDeltaTime, 0)
        for nObjectId, uCharactorController in pairs(tFallingController) do
            if not uCharactorController.isGrounded then
                uCharactorController:Move(vecDown)
            else
                tFalledControllers = tFalledControllers or {}
                table.insert(tFalledControllers, nObjectId)
            end
        end
        if tFalledControllers ~= nil then
            for _, nObjectId in ipairs(tFalledControllers) do
                tFallingController[nObjectId] = nil
            end
            if TableUtil.isEmpty(tFallingController) then
                tFallingController = nil
            end
            tFalledControllers = nil
        end
    end
end