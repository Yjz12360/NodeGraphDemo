

function SvrHandler(tNodeGraph, tNodeData)
    local nNodeId = tNodeData.nNodeId
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nRefreshId = tNodeData.nRefreshId
    local tMonster = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, nRefreshId)
    if tMonster == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId)
        return
    end
    NodeGraphEventMod.registerNode(tNodeGraph, nNodeId, Const.EventType.BeforeMonsterDead, nRefreshId)
end

function SvrBeforeMonsterDead(tNodeGraph, tNodeData, nRefreshId)
    if nRefreshId == tNodeData.nRefreshId then
        local nNodeId = tNodeData.nNodeId
        NodeGraphEventMod.unregisterNode(tNodeGraph, nNodeId, Const.EventType.BeforeMonsterDead, nRefreshId)
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
    end
end