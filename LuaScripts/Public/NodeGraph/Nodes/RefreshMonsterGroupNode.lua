
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nRefreshId = tNodeData.nRefreshId
    SvrGameRoleMod.refreshMonsterGroup(tSvrGame, nRefreshId)
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true)
end
