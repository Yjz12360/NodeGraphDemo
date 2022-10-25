
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if SvrGameRoleMod.hasMonster(tSvrGame) then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, 1)
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, 2)
    end
end
