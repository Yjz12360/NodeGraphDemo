-- require "PrintMod"
-- CltNodeGraphMod = require "CltNodeGraphMod"
-- SvrNodeGraphMod = require "SvrNodeGraphMod"

function CltHandler(tNodeGraph, tNodeData)
    local sContext = tNodeData.sContext
    if tNodeData.bIsError then
        printError(sContext)
    else
        print(sContext)
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

-- return {
--     CltHandler = CltHandler
-- }