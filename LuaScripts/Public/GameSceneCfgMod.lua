
function getConfigByName(configName)
    if Config and Config.GameScene then
        local nodeGraphData = Config.GameScene[configName]
        if nodeGraphData ~= nil then
            return nodeGraphData
        end
    end
    local sModuleName = string.format("Public/Config/GameScene/%s", configName)
    local bOK, sErrorMsg = pcall(require, sModuleName)
    if not bOK then
        printError(sErrorMsg)
        return
    end
    if Config == nil or Config.GameScene == nil then
        return 
    end
    return Config.GameScene[configName]
end

function getRefreshMonsterConfig(tGameSceneConfig, nRefreshId)
    if tGameSceneConfig == nil or tGameSceneConfig.tRefreshMonsters == nil then
        return
    end
    return tGameSceneConfig.tRefreshMonsters[nRefreshId]
end

function getRefreshMonsterGroup(tGameSceneConfig, nRefreshId)
    if tGameSceneConfig == nil or tGameSceneConfig.tRefreshMonsterGroups == nil then
        return
    end
    local tRefreshMonsterGroups = tGameSceneConfig.tRefreshMonsterGroups[nRefreshId]
    if tRefreshMonsterGroups == nil then
        return
    end
    return tRefreshMonsterGroups.tRefreshMonsters
end

function getPosition(tGameSceneConfig, nPosId)
    if tGameSceneConfig == nil then
        return
    end
    local tPositions = tGameSceneConfig.tPositions
    if tPositions == nil then
        return
    end
    return tPositions[nPosId]
end
