
local goCamera

local nHeight = 9
local nDistance = 8
local nAngle = 50

local tFollowObject
local bDisableFollow = false

function init()
    goCamera = UE.GameObject.Find("Main Camera")
    if goCamera == nil then
        printError("init camera error: camera object not found.")
    end
    goCamera.transform.localRotation = UE.Quaternion.Euler(nAngle, 0, 0)
end

function setFollowObject(tGameObject)
    if bDisableFollow then
        return
    end
    if tGameObject == nil then
        return
    end
    local goInstance = tGameObject.goInstance
    if goInstance == nil then
        return
    end
    if goCamera == nil then
        return
    end
    tFollowObject = tGameObject
    CltCameraMod.updatePos()
end

function updatePos()
    if bDisableFollow then
        return
    end
    if goCamera == nil or tFollowObject == nil then
        return
    end
    local goInstance = tFollowObject.goInstance
    if goInstance == nil then
        return
    end
    local vecTargetPos = goInstance.transform.position
    local nCamX = vecTargetPos.x
    local nCamY = vecTargetPos.y + nHeight
    local nCamZ = vecTargetPos.z - nDistance
    goCamera.transform.position = UE.Vector3(nCamX, nCamY, nCamZ)
    goCamera.transform.localRotation = UE.Quaternion.Euler(nAngle, 0, 0)
end

function setDisableFollow(bDisable)
    bDisableFollow = bDisable
end

function setPosition(nPosX, nPosY, nPosZ)
    if goCamera == nil then
        return
    end
    goCamera.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
end

function setForward(nDirX, nDirY, nDirZ)
    if goCamera == nil then
        return
    end
    goCamera.transform.forward = UE.Vector3(nDirX, nDirY, nDirZ)
end