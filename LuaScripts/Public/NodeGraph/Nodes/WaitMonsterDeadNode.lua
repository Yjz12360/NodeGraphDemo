

function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local sRefreshId = tNodeData.sRefreshId
    local tMonster = SvrGameMod.getMonsterByRefreshId(tSvrGame, sRefreshId)
    if tMonster == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

function SvrBeforeMonsterDead(tNodeGraph, tNodeData, nObjectId)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tMonster = SvrGameMod.getObject(tSvrGame, nObjectId)
    if tMonster == nil then return end
    if tMonster.nObjectType == Const.GameObjectType.Monster then
        if tMonster.sRefreshId == tNodeData.sRefreshId then
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
end