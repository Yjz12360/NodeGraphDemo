
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local sRefreshId = tNodeData.sRefreshId
    SvrGameMod.refreshMonsterGroup(tSvrGame, sRefreshId)
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
