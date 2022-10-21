
function CltHandler(tNodeGraph, tNodeData)
    local sStartPos = tNodeData.sStartPos
    local sEndPos = tNodeData.sEndPos
    local nMoveTime = tNodeData.nMoveTime
    local nMoveTimer = 0
    -- TODO sStartPos sEndPos 
    local nStartX, nStartY, nStartZ = 0, 0, 0
    local nEndX, nEndY, nEndZ = 0, 0, 0
    local nMoveX, nMoveY, nMoveZ = VectorUtil.sub(nEndX, nEndY, nEndZ, nStartX, nStartY, nStartZ)
    local nDirX, nDirY, nDirZ = VectorUtil.normalize(nMoveX, nMoveY, nMoveZ)
    CltCameraMod.setDisableFollow(true)
    CltCameraMod.setPosition(nStartX, nStartY, nStartZ)
    CltCameraMod.setForward(nDirX, nDirY, nDirZ)
    local nUpdateId
    nUpdateId = CltUpdateManagerMod.register(function(nDeltaTime)
        nMoveTimer = nMoveTimer + nDeltaTime
        if nMoveTimer >= nMoveTime then
            CltCameraMod.setDisableFollow(false)
            CltCameraMod.updatePos()
            CltUpdateManagerMod.unregister(nUpdateId)
            return
        end
        local nRate = nMoveTimer / nMoveTime
        local nPathX, nPathY, nPathZ = VectorUtil.mul(nMoveX, nMoveY, nMoveZ, nRate)
        local nCurrX, nCurrY, nCurrZ = VectorUtil.add(nStartX, nStartY, nStartZ, nPathX, nPathY, nPathZ)
        CltCameraMod.setPosition(nCurrX, nCurrY, nCurrZ)
    end)
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nTime = tNodeData.nTime
    TimerMod.delay(nTime, function()
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end)
end