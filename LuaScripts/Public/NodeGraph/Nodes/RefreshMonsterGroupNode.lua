
function SvrHandler(tNodeGraph, tNodeData)
    local sRefreshId = tNodeData.sRefreshId
    SvrGameMod.refreshMonsterGroup(tNodeGraph.tSvrGame, sRefreshId)
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
