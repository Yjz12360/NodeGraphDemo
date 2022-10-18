
local CompareType = {
    LessThan = 1,
    MoreThan = 2,
    Equal = 3,
}

function SvrOnCheck(tNodeGraph, tNodeData)
    local nMonsterNum = SvrGameMod.getMonsterNum(tNodeGraph.tSvrGame)
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

