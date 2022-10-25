
function CltHandler(tNodeGraph, tNodeData)
    local nEffectId = tNodeData.nEffectId
    local sPosId = tNodeData.sPosId
    local tCltGame = CltGameMod.getGame()
    local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sPosId)
    if tPos ~= nil then
        CltEffectMod.playEffect(nEffectId, tPos.x, tPos.y, tPos.z)
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end

