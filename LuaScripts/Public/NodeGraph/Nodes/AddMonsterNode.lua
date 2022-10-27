
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nRefreshId = tNodeData.nRefreshId
    if nRefreshId and nRefreshId ~= "" then
        SvrGameRoleMod.refreshMonster(tSvrGame, nRefreshId)
    else
        local nConfigId = tNodeData.nConfigId
        local nPosX, nPosY, nPosZ = 0, 0, 0
        local nPosId = tNodeData.nPosId
        if nPosId ~= nil and nPosId ~= "" then
            local tPos = GameSceneCfgMod.getPosition(tSvrGame.tGameSceneConfig, nPosId)
            if tPos ~= nil then
                nPosX, nPosY, nPosZ = tPos.x, tPos.y, tPos.z
            end
        else
            nPosX, nPosY, nPosZ = tNodeData.nPosX, tNodeData.nPosY, tNodeData.nPosZ
        end
        SvrGameRoleMod.addMonster(tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true)
end
