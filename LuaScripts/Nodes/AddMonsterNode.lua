
function SvrHandler(tNodeGraph, tNodeData)
    local nRefreshId = tNodeData.nRefreshId
    if nRefreshId > 0 then
        SvrGameMod.addSceneMonster(tNodeGraph.tSvrGame, nRefreshId)
    else
        local nConfigId = tNodeData.nConfigId
        local nPosX = tNodeData.nPosX
        local nPosY = tNodeData.nPosY
        local nPosZ = tNodeData.nPosZ
        SvrGameMod.addMonster(tNodeGraph.tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
