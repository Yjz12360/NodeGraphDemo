

function SvrHandler(tNodeGraph, tNodeData)
    local sRefreshId = tNodeData.sRefreshId
    local tMonster = SvrGameMod.getMonsterByRefreshId(tNodeGraph.tSvrGame, sRefreshId)
    if tMonster == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

function SvrBeforeMonsterDead(tNodeGraph, tNodeData, nObjectId)
    local tMonster = SvrGameMod.getObject(tNodeGraph.tSvrGame, nObjectId)
    if tMonster == nil then return end
    if tMonster.nObjectType == Const.GameObjectType.Monster then
        if tMonster.sRefreshId == tNodeData.sRefreshId then
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
end