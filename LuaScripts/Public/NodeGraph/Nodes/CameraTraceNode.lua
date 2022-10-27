
function CltHandler(tNodeGraph, tNodeData)
    local nStartPosId = tNodeData.nStartPosId
    local nEndPosId = tNodeData.nEndPosId
    local nMoveTime = tNodeData.nMoveTime or 1
    local tCltGame = CltGameMod.getGame()
    local nStartX, nStartY, nStartZ = 0, 0, 0
    local tStartPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, nStartPosId)
    if tStartPos ~= nil then
        nStartX, nStartY, nStartZ = tStartPos.x, tStartPos.y, tStartPos.z
    end
    local nEndX, nEndY, nEndZ = 0, 0, 0
    local tEndPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, nEndPosId)
    if tEndPos ~= nil then
        nEndX, nEndY, nEndZ = tEndPos.x, tEndPos.y, tEndPos.z
    end
    local goOriFollow = CltCameraMod.getFollowObject()
    CltCameraMod.setTrace(nStartX, nStartY, nStartZ, nEndX, nEndY, nEndZ, nMoveTime, function()
        CltCameraMod.setFollowObject(goOriFollow)
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
    end)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nMoveTime = tNodeData.nMoveTime or 1
    TimerMod.delay(nMoveTime, function()
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
    end)
end