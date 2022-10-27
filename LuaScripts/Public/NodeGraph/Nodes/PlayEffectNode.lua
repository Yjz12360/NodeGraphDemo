
function CltHandler(tNodeGraph, tNodeData)
    local nEffectId = tNodeData.nEffectId
    local nPosId = tNodeData.nPosId
    local tCltGame = CltGameMod.getGame()
    local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, nPosId)
    if tPos ~= nil then
        CltEffectMod.playEffect(nEffectId, tPos.x, tPos.y, tPos.z)
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end

