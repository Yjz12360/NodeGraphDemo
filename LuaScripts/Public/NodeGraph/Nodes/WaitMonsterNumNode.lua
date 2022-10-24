
local CompareType = {
    LessThan = 1,
    MoreThan = 2,
    Equal = 3,
}

function SvrOnCheck(tNodeGraph, tNodeData)
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
                if tGameObject.sGroupId == sMonsterGroupId then
                    nMonsterNum = nMonsterNum + 1
                end
            end
        end
    end
    local nCompareType = tNodeData.nCompareType or CompareType.LessThan
    local nCompareNum = tNodeData.nNum
    local bTrigger
    if nCompareType == CompareType.LessThan then
        bTrigger = nMonsterNum < nCompareNum
    elseif nCompareType == CompareType.MoreThan then
        bTrigger = nMonsterNum > nCompareNum
    elseif nCompareType == CompareType.Equal then
        bTrigger = nMonsterNum == nCompareNum
    else
        bTrigger = false
    end
    if bTrigger then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
    end
end

