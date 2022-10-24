
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

function addMonster(tSvrGame, nConfigId, nPosX, nPosY, nPosZ, sRefreshId)
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
    tMonster.sRefreshId = sRefreshId
    tSvrGame.tGameObjects[nObjectId] = tMonster
    AIManagerMod.addAI(tSvrGame.tAIManager, nObjectId)
    Messager.S2CAddMonster(tSvrGame.nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, sRefreshId)
    SvrGameMod.processEvent(tSvrGame, Const.EventType.AddMonster)
    return tMonster
end

function refreshMonster(tSvrGame, sRefreshId)
    if tSvrGame == nil then
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshConfig = GameSceneCfgMod.getRefreshMonsterConfig(tGameSceneConfig, sRefreshId)
    if tRefreshConfig == nil then
        printError("refreshMonster error: tRefreshConfig not exist. sRefreshId: " .. sRefreshId)
        return
    end
    local nMonsterCfgId = tRefreshConfig.nMonsterCfgId
    local tPos = tRefreshConfig.tPos
    local tMonster = SvrGameRoleMod.addMonster(tSvrGame, nMonsterCfgId, tPos.x, tPos.y, tPos.z, sRefreshId)
    local tPath = tRefreshConfig.tPath
    if tPath ~= nil then
        AIManagerMod.setPath(tSvrGame.tAIManager, tMonster.nObjectId, tPath)
    end
    return tMonster
end

function refreshMonsterGroup(tSvrGame, sGroupId)
    if tSvrGame == nil then
        return
    end
    local tGameSceneConfig = tSvrGame.tGameSceneConfig
    local tRefreshMonsterGroups = GameSceneCfgMod.getRefreshMonsterGroup(tGameSceneConfig, sGroupId)
    if tRefreshMonsterGroups == nil then
        printError("refreshMonsterGroup error: config not exist. sGroupId: " .. sGroupId)
        return
    end
    for _, sRefreshId in ipairs(tRefreshMonsterGroups) do
        SvrGameRoleMod.refreshMonster(tSvrGame, sRefreshId)
    end
end

function roleDead(tSvrGame, nObjectId)
    if tSvrGame == nil then
        return
    end
    local tRole = tSvrGame.tGameObjects[nObjectId]
    if tRole == nil then
        return
    end

    local tGameObject = tSvrGame.tGameObjects[nObjectId]
    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        SvrGameMod.processEvent(tSvrGame, Const.EventType.BeforeMonsterDead, nObjectId)
    end

    tSvrGame.tGameObjects[nObjectId] = nil
    Messager.S2CRoleDead(tSvrGame.nGameId, nObjectId)

    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        AIManagerMod.delAI(tSvrGame.tAIManager, nObjectId)
        SvrGameMod.processEvent(tSvrGame, Const.EventType.MonsterDead, nObjectId)
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

function getMonsterByRefreshId(tSvrGame, sRefreshId)
    if tSvrGame == nil then
        return
    end
    for _, tGameObject in pairs(tSvrGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            if tGameObject.sRefreshId == sRefreshId then
                return tGameObject
            end
        end
    end
end