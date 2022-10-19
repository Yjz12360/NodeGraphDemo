
function SvrHandler(tNodeGraph, tNodeData)
    local sRefreshId = tNodeData.sRefreshId
    if sRefreshId and sRefreshId ~= "" then
        SvrGameMod.refreshMonster(tNodeGraph.tSvrGame, sRefreshId)
    else
        local nConfigId = tNodeData.nConfigId
        local nPosX = tNodeData.nPosX
        local nPosY = tNodeData.nPosY
        local nPosZ = tNodeData.nPosZ
        SvrGameMod.addMonster(tNodeGraph.tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
