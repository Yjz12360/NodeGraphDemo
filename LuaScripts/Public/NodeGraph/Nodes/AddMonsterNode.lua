
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local sRefreshId = tNodeData.sRefreshId
    if sRefreshId and sRefreshId ~= "" then
        SvrGameMod.refreshMonster(tSvrGame, sRefreshId)
    else
        local nConfigId = tNodeData.nConfigId
        local nPosX = tNodeData.nPosX
        local nPosY = tNodeData.nPosY
        local nPosZ = tNodeData.nPosZ
        SvrGameMod.addMonster(tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
