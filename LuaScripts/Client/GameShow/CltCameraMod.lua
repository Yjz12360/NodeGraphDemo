
local goCamera

local nCameraMode = Const.CameraMode.None

local nUpdateId

-- follow
local nHeight = 9
local nDistance = 8
local nAngle = 50
local tFollowObject
local goFollow = nil

--trace
local nTraceStartX, nTraceStartY, nTraceStartZ
local nTraceMoveX, nTraceMoveY, nTraceMoveZ
local nTraceTime, nTraceTimer
local fTraceCb

function init()
    goCamera = UE.GameObject.Find("Main Camera")
    if goCamera == nil then
        printError("init camera error: camera object not found.")
    end
    goCamera.transform.localRotation = UE.Quaternion.Euler(nAngle, 0, 0)
end

function setFollowObject(tGameObject)
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
    nCameraMode = Const.CameraMode.Follow
    tFollowObject = tGameObject
    goFollow = tGameObject.goInstance
    if nUpdateId ~= nil then
        CltUpdateManagerMod.unregisterMod("CltCameraMod")
        nUpdateId = nil
    end
    CltCameraMod.update()
end

function getFollowObject()
    return tFollowObject
end

function setTrace(nStartX, nStartY, nStartZ, nEndX, nEndY, nEndZ, nTime, fCallback)
    if nStartX == nil or nStartY == nil or nStartZ == nil or
        nEndX == nil or nEndY == nil or nEndZ == nil then
        return
    end
    if goCamera == nil then
        return
    end
    nCameraMode = Const.CameraMode.Trace
    nTraceStartX, nTraceStartY, nTraceStartZ = nStartX, nStartY, nStartZ
    nTraceTime = nTime
    fTraceCb = fCallback
    nTraceTimer = 0
    nTraceMoveX, nTraceMoveY, nTraceMoveZ = VectorUtil.sub(nEndX, nEndY, nEndZ, nStartX, nStartY, nStartZ)
    local nDirX, nDirY, nDirZ = VectorUtil.normalize(nTraceMoveX, nTraceMoveY, nTraceMoveZ)
    goCamera.transform.forward = UE.Vector3(nDirX, nDirY, nDirZ)
    CltCameraMod.update()
    nUpdateId = CltUpdateManagerMod.registerMod("CltCameraMod")
end

function update(nDeltaTime)
    if goCamera == nil then
        return
    end
    nDeltaTime = nDeltaTime or 0
    if nCameraMode == Const.CameraMode.Follow then
        if goFollow ~= nil then
            local vecTargetPos = goFollow.transform.position
            local nCamX = vecTargetPos.x
            local nCamY = vecTargetPos.y + nHeight
            local nCamZ = vecTargetPos.z - nDistance
            goCamera.transform.position = UE.Vector3(nCamX, nCamY, nCamZ)
            goCamera.transform.localRotation = UE.Quaternion.Euler(nAngle, 0, 0)
        end
    elseif nCameraMode == Const.CameraMode.Trace then
        nTraceTimer = nTraceTimer + nDeltaTime
        local nRate = nTraceTimer / nTraceTime
        if nRate >= 1 then
            if fTraceCb ~= nil then
                fTraceCb()
                fTraceCb = nil
            end
            if nUpdateId ~= nil then
                CltUpdateManagerMod.unregisterMod("CltCameraMod")
                nUpdateId = nil
            end
        else
            local nPathX, nPathY, nPathZ = VectorUtil.mul(nTraceMoveX, nTraceMoveY, nTraceMoveZ, nRate)
            local nCurrX, nCurrY, nCurrZ = VectorUtil.add(nTraceStartX, nTraceStartY, nTraceStartZ, nPathX, nPathY, nPathZ)
            goCamera.transform.position = UE.Vector3(nCurrX, nCurrY, nCurrZ)
        end
    end
end
