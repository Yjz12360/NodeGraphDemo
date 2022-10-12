local SvrNodeGraphMod = {}
package.loaded["SvrNodeGraphMod"] = SvrNodeGraphMod

require "PrintMod"
NodeGraphCfgMod = require "NodeGraphCfgMod"
NodesHandlerMod = require "NodesHandlerMod"
Messager = require "Messager"
TableUtil = require "TableUtil"


local NodeGraphState = {
    Pending = 1,
    Running = 2,
    Finish = 3,
}

-- local nCurrId = 0
-- local function getNextId()
--     nCurrId = nCurrId + 1
--     return nCurrId
-- end

function addNodeGraph(tSvrGame, nNodeGraphId, nConfigId)
    local tConfig = Config.NodeGraph[nConfigId]
    if tConfig == nil or tConfig.sName == nil then
        printError("addNodeGraph error nConfigId: " .. nConfigId)
        return
    end
    local tNodeGraph = {}
    tNodeGraph.tGame = tSvrGame
    tNodeGraph.nNodeGraphId = nNodeGraphId
    tNodeGraph.tConfigData = NodeGraphCfgMod.getConfigByName(tConfig.sName)
    tNodeGraph.nState = NodeGraphState.Pending
    tNodeGraph.tRunningNodes = {}
    tNodeGraph.tPendingNodes = {}
    tNodeGraph.tRemoveNodes = {}
    return tNodeGraph
end

function startNodeGraph(tNodeGraph)
    if tNodeGraph == nil or tNodeGraph.tConfigData == nil then
        return
    end
    TableUtil.clear(tNodeGraph.tPendingNodes)
    TableUtil.clear(tNodeGraph.tRemoveNodes)
    tNodeGraph.nState = NodeGraphState.Running
    local sStartNodeId = tNodeGraph.tConfigData.sStartNodeId
    tNodeGraph.tPendingNodes[sStartNodeId] = true
end

function updateNodeGraph(tNodeGraph, nDeltaTime)
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= NodeGraphState.Running then
        return
    end
    local tClonePendingNodes = {}
    for sNodeId, _ in pairs(tNodeGraph.tPendingNodes) do
        tClonePendingNodes[sNodeId] = true
    end
    TableUtil.clear(tNodeGraph.tPendingNodes)
    for sNodeId, _ in pairs(tClonePendingNodes) do
        SvrNodeGraphMod.triggerNode(tNodeGraph, sNodeId)
    end
    for sNodeId, _ in pairs(tNodeGraph.tRunningNodes) do
        local fUpdate = tNodeGraph.tRunningNodes[sNodeId]
        if fUpdate ~= nil then
            fUpdate(nDeltaTime)
        end
    end
    for sNodeId, _ in pairs(tNodeGraph.tRemoveNodes) do
        tNodeGraph.tRunningNodes[sNodeId] = nil
    end
    TableUtil.clear(tNodeGraph.tRemoveNodes)
    if(TableUtil.isEmpty(tNodeGraph.tRunningNodes) and TableUtil.isEmpty(tNodeGraph.tPendingNodes)) then
        tNodeGraph.nState = NodeGraphState.Finish
    end
end

function triggerNode(tNodeGraph, sNodeId)
    if tNodeGraph == nil then
        return
    end
    local tNodeData = NodeGraphCfgMod.getNodeConfig(tNodeGraph.tConfigData, sNodeId)
    if tNodeData == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end
    if tNodeData.nNodeType == NodeType.Start then
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
        return
    end
    local fHandler = NodesHandlerMod.getSvrNodeHandler(tNodeData.nNodeType)
    if fHandler ~= nil then
        local fUpdate = fHandler(tNodeGraph, tNodeData)
        if fUpdate ~= nil then
            tNodeGraph.tRunningNodes[sNodeId] = fUpdate
        end
    else
        SvrNodeGraphMod.finishNode(tNodeGraph, sNodeId)
    end
end

function finishNode(tNodeGraph, sNodeId, nPath)
    nPath = nPath or 1
    if tNodeGraph == nil then
        return
    end
    if tNodeGraph.nState ~= NodeGraphState.Running then
        return
    end
    tNodeGraph.tRemoveNodes[sNodeId] = true
    local tTransitions = NodeGraphCfgMod.getTransitions(tNodeGraph.tConfigData)
    for _, tTransition in pairs(tTransitions) do
        if tTransition.sFromNodeId == sNodeId and tTransition.nPath == nPath then
            tNodeGraph.tPendingNodes[tTransition.sToNodeId] = true
        end
    end
    Messager.S2CFinishNode(tNodeGraph.nNodeGraphId, sNodeId, nPath)
end

function isFinish(tNodeGraph)
    return tNodeGraph.nState == NodeGraphState.Finish
end


SvrNodeGraphMod.addNodeGraph = addNodeGraph
SvrNodeGraphMod.getNodeGraphById = getNodeGraphById
SvrNodeGraphMod.startNodeGraph = startNodeGraph
SvrNodeGraphMod.updateNodeGraph = updateNodeGraph
SvrNodeGraphMod.triggerNode = triggerNode
SvrNodeGraphMod.finishNode = finishNode
SvrNodeGraphMod.isFinish = isFinish
return SvrNodeGraphMod