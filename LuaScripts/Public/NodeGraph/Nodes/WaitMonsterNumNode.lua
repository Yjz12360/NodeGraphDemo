
local CompareType = {
    LessThan = 1,
    MoreThan = 2,
    Equal = 3,
}

local function doCheck(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nMonsterNum
    local sMonsterGroupId = tNodeData.sMonsterGroupId
    if sMonsterGroupId == "" then
        local nMonsterNum = SvrGameRoleMod.getMonsterNum(tSvrGame)        
    else
        nMonsterNum = 0
        local tGameObjects = SvrGameMod.getObjects(tSvrGame)
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Monster then
                if tGameObject.nGroupId == sMonsterGroupId then
                    nMonsterNum = nMonsterNum + 1
                end
            end
        end
    end
    local nCompareType = tNodeData.nCompareType or CompareType.LessThan
    local nCompareNum = tNodeData.nNum
    local bCheck
    if nCompareType == CompareType.LessThan then
        bCheck = nMonsterNum < nCompareNum
    elseif nCompareType == CompareType.MoreThan then
        bCheck = nMonsterNum > nCompareNum
    elseif nCompareType == CompareType.Equal then
        bCheck = nMonsterNum == nCompareNum
    else
        bCheck = false
    end
    return bTrigger
end

function SvrHandler(tNodeGraph, tNodeData)
    if doCheck(tNodeGraph, tNodeData) then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
        return
    end
    local nNodeId = tNodeData.nNodeId
    NodeGraphEventMod.registerNode(tNodeGraph, nNodeId, Const.EventType.AddMonster)
    NodeGraphEventMod.registerNode(tNodeGraph, nNodeId, Const.EventType.MonsterDead)
end

function SvrOnMonsterNumChange(tNodeGraph, tNodeData)
    if doCheck(tNodeGraph, tNodeData) then
        local nNodeId = tNodeData.nNodeId
        NodeGraphEventMod.unregisterNode(tNodeGraph, nNodeId, Const.EventType.AddMonster)
        NodeGraphEventMod.unregisterNode(tNodeGraph, nNodeId, Const.EventType.MonsterDead)
        SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId)
    end
end

