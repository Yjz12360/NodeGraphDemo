
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local sRefreshId = tNodeData.sRefreshId
    SvrGameRoleMod.refreshMonsterGroup(tSvrGame, sRefreshId)
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end
