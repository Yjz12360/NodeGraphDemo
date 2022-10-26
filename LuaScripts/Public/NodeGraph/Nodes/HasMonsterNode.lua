
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if SvrGameRoleMod.hasMonster(tSvrGame) then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true, 1)
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true, 2)
    end
end
