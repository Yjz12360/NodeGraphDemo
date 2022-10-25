
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

function getNodeConfig(tNodeGraphConfig, sNodeId)
    if tNodeGraphConfig == nil or tNodeGraphConfig.tNodeMap == nil then
        return
    end
    return tNodeGraphConfig.tNodeMap[sNodeId]
end

function getNodeCount(tNodeGraphConfig)
    if tNodeGraphConfig == nil or tNodeGraphConfig.tNodeMap == nil then
        return 0
    end
    return TableUtil.dictCount(tNodeGraphConfig.tNodeMap)
end

function getTransitions(tNodeGraphConfig, sNodeId)
    if tNodeGraphConfig == nil then
        return
    end
    local tTransitions = tNodeGraphConfig.tTransitions
    if tTransitions == nil then
        return
    end
    return tTransitions[sNodeId]
end
