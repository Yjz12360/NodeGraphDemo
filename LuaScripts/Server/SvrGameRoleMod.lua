
function addPlayer(tSvrGame, nConfigId, nPosX, nPosY, nPosZ)
    if tSvrGame == nil then
        return
    end
    local tConfig = Config.Player[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local nObjectId = SvrGameMod.getNextObjectId(tSvrGame)
    local tPlayer = {}
    tPlayer.nObjectId = nObjectId
    tPlayer.nObjectType = Const.GameObjectType.Player
    tPlayer.tConfig = tConfig
    tPlayer.nPosX = nPosX
    tPlayer.nPosY = nPosY
    tPlayer.nPosZ = nPosZ
    tPlayer.nCurrHP = tConfig.nHP
    tSvrGame.tGameObjects[nObjectId] = tPlayer
    Messager.S2CAddPlayer(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
end

function addMonster(tSvrGame, nConfigId, nPosX, nPosY, nPosZ, nRefreshId)
    if tSvrGame == nil then
        return
    end
    local tConfig = Config.Monster[nConfigId]
    if tConfig == nil then
        printError("MonsterConfig not exist configId: " .. nConfigId)
        return
    end
    local nObjectId = SvrGameMod.getNextObjectId(tSvrGame)
    local tMonster = {}
    tMonster.nObjectId = nObjectId
    tMonster.nObjectType = Const.GameObjectType.Monster
    tMonster.tConfig = tConfig
    tMonster.nPosX = nPosX
    tMonster.nPosY = nPosY
    tMonster.nPosZ = nPosZ
    tMonster.nCurrHP = tConfig.nHP
    tMonster.nRefreshId = nRefreshId
    tSvrGame.tGameObjects[nObjectId] = tMonster
    AIManagerMod.addAI(tSvrGame.tAIManager, nObjectId)
    Messager.S2CAddMonster(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, nRefreshId)
    SvrGameMod.processEvent(tSvrGame, Const.EventType.AddMonster, nRefreshId)
    return tMonster
end

function refreshMonster(tSvrGame, nRefreshId)
    if tSvrGame == nil then
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshConfig = GameSceneCfgMod.getRefreshMonsterConfig(tGameSceneConfig, nRefreshId)
    if tRefreshConfig == nil then
        printError("refreshMonster error: tRefreshConfig not exist. nRefreshId: " .. nRefreshId)
        return
    end
    local nMonsterCfgId = tRefreshConfig.nMonsterCfgId
    local tPos = tRefreshConfig.tPos
    local tMonster = SvrGameRoleMod.addMonster(tSvrGame, nMonsterCfgId, tPos.x, tPos.y, tPos.z, nRefreshId)
    local tPath = tRefreshConfig.tPath
    if tPath ~= nil then
        AIManagerMod.setPath(tSvrGame.tAIManager, tMonster.nObjectId, tPath)
    end
    return tMonster
end

function refreshMonsterGroup(tSvrGame, nGroupId)
    if tSvrGame == nil then
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshMonsterGroups = GameSceneCfgMod.getRefreshMonsterGroup(tGameSceneConfig, nGroupId)
    if tRefreshMonsterGroups == nil then
        printError("refreshMonsterGroup error: config not exist. nGroupId: " .. nGroupId)
        return
    end
    for _, nRefreshId in ipairs(tRefreshMonsterGroups) do
        SvrGameRoleMod.refreshMonster(tSvrGame, nRefreshId)
    end
end

function roleDead(tSvrGame, nObjectId)
    if tSvrGame == nil then
        return
    end
    local tGameObject = tSvrGame.tGameObjects[nObjectId]
    if tGameObject == nil then
        return
    end

    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        local nRefreshId = tGameObject.nRefreshId
        SvrGameMod.processEvent(tSvrGame, Const.EventType.BeforeMonsterDead, nRefreshId)
    end

    tSvrGame.tGameObjects[nObjectId] = nil
    Messager.S2CRoleDead(tSvrGame.nGameId, nObjectId)

    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        AIManagerMod.delAI(tSvrGame.tAIManager, nObjectId)
        local nRefreshId = tGameObject.nRefreshId
        SvrGameMod.processEvent(tSvrGame, Const.EventType.MonsterDead, nRefreshId)
    end
end

function hasMonster(tSvrGame)
    if tSvrGame == nil then
        return false
    end
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            return true
        end
    end
    return false
end

function getMonsterNum(tSvrGame)
    if tSvrGame == nil then
        return 0
    end
    local nCount = 0
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            nCount = nCount + 1
        end
    end
    return nCount
end

function getMonsterByRefreshId(tSvrGame, nRefreshId)
    if tSvrGame == nil then
        return
    end
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            if tGameObject.nRefreshId == nRefreshId then
                return tGameObject
            end
        end
    end
end

function forceSetPos(tSvrGame, nObjectId, nPosX, nPosY, nPosZ)
    if tSvrGame == nil then
        return
    end
    local tGameObject = tSvrGame.tGameObjects[nObjectId]
    if nObjectId == nil then
        return
    end
    tGameObject.nPosX = nPosX
    tGameObject.nPosY = nPosY
    tGameObject.nPosZ = nPosZ
    Messager.S2CForceSetPos(tSvrGame.nGameId, nObjectId, nPosX, nPosY, nPosZ)
end