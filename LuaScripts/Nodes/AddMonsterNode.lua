
function SvrHandler(tNodeGraph, tNodeData)
    -- local sContext = tNodeData.sContext
    -- if tNodeData.bIsError then
    --     printError(sContext)
    -- else
    --     print(sContext)
    -- end
    local nConfigId = tNodeData.nConfigId
    local nPosX = tNodeData.nPosX
    local nPosY = tNodeData.nPosY
    local nPosZ = tNodeData.nPosZ
    SvrGameMod.addMonster(tNodeGraph.tSvrGame, nConfigId, nPosX, nPosY, nPosZ)

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
