
local SvrNodeGraphCfgMod = {}
package.loaded["SvrNodeGraphCfgMod"] = SvrNodeGraphCfgMod

-- require "NodeGraphCfgMod"
-- require "SvrSceneNodeGraphMod"
require "PrintMod"

-- local tNodeGraphs = {}

function getConfigByName(configName)
    if Config and Config.NodeGraphData then
        local nodeGraphData = Config.NodeGraphData[configName]
        if nodeGraphData ~= nil then
            return nodeGraphData
        end
    end
    -- local tConfig = tNodeGraphs[configName]
    -- if tConfig ~= nil then
    --     return tConfig
    -- end
    local sModuleName = string.format("Config/NodeGraphData/%s", configName)
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

function getTransitions(tNodeGraphConfig)
    if tNodeGraphConfig == nil then
        return
    end
    return tNodeGraphConfig.tTransitions
end

SvrNodeGraphCfgMod.getConfigByName = getConfigByName
SvrNodeGraphCfgMod.getNodeConfig = getNodeConfig
SvrNodeGraphCfgMod.getTransitions = getTransitions
return SvrNodeGraphCfgMod