
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

function checkRequireOutput(tNodeGraphConfig, nNodeId, sOutputAttr)
    if tNodeGraphConfig == nil then
        return false
    end
    local tRequiredOutput = tNodeGraphConfig.tRequiredOutput
    if tRequiredOutput == nil then
        return false
    end
    local tNodeRequireOutput = tRequiredOutput[nNodeId]
    if tNodeRequireOutput == nil then
        return false
    end
    return tNodeRequireOutput[sOutputAttr] ~= nil
end

function getInputSource(tNodeGraphConfig, nNodeId, sInputAttr)
    if tNodeGraphConfig == nil then
        return
    end
    local tNodeInputSource = tNodeGraphConfig.tNodeInputSource
    if tNodeInputSource == nil then
        return
    end
    local tSource = tNodeInputSource[nNodeId]
    if tSource == nil then
        return
    end
    local tAttrSource = tSource[sInputAttr]
    if tAttrSource == nil then
        return
    end
    return tAttrSource.nInputNodeId, tAttrSource.sInputNodeAttr
end