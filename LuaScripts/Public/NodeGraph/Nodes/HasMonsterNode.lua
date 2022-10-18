
function SvrHandler(tNodeGraph, tNodeData)
    -- local sContext = tNodeData.sContext
    -- if tNodeData.bIsError then
    --     printError(sContext)
    -- else
    --     print(sContext)
    -- end
    if SvrGameMod.hasMonster(tNodeGraph.tSvrGame) then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId, 1)
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId, 2)
    end
end
