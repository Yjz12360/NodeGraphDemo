
function CltHandler(tNodeGraph, tNodeData)
    local sStartPosId = tNodeData.sStartPosId
    local sEndPosId = tNodeData.sEndPosId
    local nMoveTime = tNodeData.nMoveTime or 1
    local tCltGame = CltGameMod.getGame()
    local nStartX, nStartY, nStartZ = 0, 0, 0
    local tStartPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sStartPosId)
    if tStartPos ~= nil then
        nStartX, nStartY, nStartZ = tStartPos.x, tStartPos.y, tStartPos.z
    end
    local nEndX, nEndY, nEndZ = 0, 0, 0
    local tEndPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sEndPosId)
    if tEndPos ~= nil then
        nEndX, nEndY, nEndZ = tEndPos.x, tEndPos.y, tEndPos.z
    end
    local nMoveX, nMoveY, nMoveZ = VectorUtil.sub(nEndX, nEndY, nEndZ, nStartX, nStartY, nStartZ)
    local nDirX, nDirY, nDirZ = VectorUtil.normalize(nMoveX, nMoveY, nMoveZ)
    CltCameraMod.setDisableFollow(true)
    CltCameraMod.setPosition(nStartX, nStartY, nStartZ)
    CltCameraMod.setForward(nDirX, nDirY, nDirZ)
    local nMoveTimer = 0
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
    local nMoveTime = tNodeData.nMoveTime or 1
    TimerMod.delay(nMoveTime, function()
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end)
end