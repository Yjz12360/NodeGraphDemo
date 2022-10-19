
function SvrHandler(tNodeGraph, tNodeData)
    local sRefreshId = tNodeData.sRefreshId
    if sRefreshId > 0 then
        SvrGameMod.addSceneMonster(tNodeGraph.tSvrGame, sRefreshId)
    else
        local nConfigId = tNodeData.nConfigId
        local nPosX = tNodeData.nPosX
        local nPosY = tNodeData.nPosY
        local nPosZ = tNodeData.nPosZ
        SvrGameMod.addMonster(tNodeGraph.tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
