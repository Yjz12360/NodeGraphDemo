
function getConfigByName(configName)
    if Config and Config.NodeGraphData then
        local nodeGraphData = Config.NodeGraphData[configName]
        if nodeGraphData ~= nil then
            return nodeGraphData
        end
    end
    local sModuleName = string.format("Public/Config/NodeGraphData/%s", configName)
    local bOK, sErrorMsg = pcall(require, sModuleName)
    if not bOK then
        printError(sErrorMsg)
        return
    end
    if Config == nil or Config.NodeGraphData == nil then
        return 
    end
    return Config.NodeGraphData[configName]
end

function getNodeConfig(tNodeGraphConfig, nNodeId)
    if tNodeGraphConfig == nil or tNodeGraphConfig.tNodeMap == nil then
        return
    end
    return tNodeGraphConfig.tNodeMap[nNodeId]
end

function getNodeCount(tNodeGraphConfig)
    if tNodeGraphConfig == nil or tNodeGraphConfig.tNodeMap == nil then
        return 0
    end
    return TableUtil.dictCount(tNodeGraphConfig.tNodeMap)
end

function getTransitionNodes(tNodeGraphConfig, nNodeId, nPath)
    if tNodeGraphConfig == nil then
        return
    end
    local tTransitions = tNodeGraphConfig.tTransitions
    if tTransitions == nil then
        return
    end
    local tNodeTransitions = tTransitions[nNodeId]
    if tNodeTransitions == nil then
        return
    end
    return tNodeTransitions[nPath]
end
