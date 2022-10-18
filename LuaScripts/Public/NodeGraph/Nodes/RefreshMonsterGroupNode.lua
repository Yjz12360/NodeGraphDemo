
function SvrHandler(tNodeGraph, tNodeData)
    local nRefreshId = tNodeData.nRefreshId
    SvrGameMod.refreshMonsterGroup(tNodeGraph.tSvrGame, nRefreshId)
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
