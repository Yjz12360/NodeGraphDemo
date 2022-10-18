

function SvrHandler(tNodeGraph, tNodeData)
    local nRefreshId = tNodeData.nRefreshId
    local tMonster = SvrGameMod.getMonsterByRefreshId(tNodeGraph.tSvrGame, nRefreshId)
    if tMonster == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

function SvrBeforeMonsterDead(tNodeGraph, tNodeData, nObjectId)
    local tMonster = SvrGameMod.getObject(tNodeGraph.tSvrGame, nObjectId)
    if tMonster == nil then return end
    if tMonster.nObjectType == Const.GameObjectType.Monster then
        if tMonster.nRefreshId == tNodeData.nRefreshId then
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        end
    end
end