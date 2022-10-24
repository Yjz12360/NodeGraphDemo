
function addPlayer(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, bLocalPlayer)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tConfig = Config.Player[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local tPlayer = {}
    tPlayer.nObjectId = nObjectId
    tPlayer.nObjectType = Const.GameObjectType.Player
    tPlayer.tConfig = tConfig
    local tCltGame = CltGameMod.getGame()
    tCltGame.tGameObjects[nObjectId] = tPlayer
    local nModelId = tConfig.nModelId
    local tModelConfig = Config.Model[nModelId]
    if tModelConfig ~= nil then
        local sPrefabFile = tModelConfig.sPrefabFile
        local goPrefab = UE.Resources.Load(sPrefabFile)
        local goInstance = UE.GameObject.Instantiate(goPrefab)
        tPlayer.goInstance = goInstance
        local goPlayerRoot = UE.GameObject.Find("GameObjects/Players")
        if goPlayerRoot ~= nil then
            goInstance.transform:SetParent(goPlayerRoot.transform)
        end
        goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
        local uGameObjectId = goInstance:GetComponent(typeof(CS.Game.GameObjectId))
        if uGameObjectId ~= nil then
            uGameObjectId.ID = nObjectId
        end
    end
    if bLocalPlayer then
        tPlayer.bLocalPlayer = true
        CltLocalPlayerMod.setLocalPlayer(tPlayer)
    end
end

function addMonster(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ, sRefreshId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tConfig = Config.Monster[nConfigId]
    if tConfig == nil then
        printError("MonsterConfig not exist configId: " .. nConfigId)
        return
    end
    local tMonster = {}
    tMonster.nObjectId = nObjectId
    tMonster.nObjectType = Const.GameObjectType.Monster
    tMonster.tConfig = tConfig
    tMonster.sRefreshId = sRefreshId
    local tCltGame = CltGameMod.getGame()
    tCltGame.tGameObjects[nObjectId] = tMonster
    local nModelId = tConfig.nModelId
    local tModelConfig = Config.Model[nModelId]
    if tModelConfig ~= nil then
        local sPrefabFile = tModelConfig.sPrefabFile
        local goPrefab = UE.Resources.Load(sPrefabFile)
        local goInstance = UE.GameObject.Instantiate(goPrefab)
        tMonster.goInstance = goInstance
        local goMonsterRoot = UE.GameObject.Find("GameObjects/Monsters")
        if goMonsterRoot ~= nil then
            goInstance.transform:SetParent(goMonsterRoot.transform)
        end
        goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
        local uGameObjectId = goInstance:GetComponent(typeof(CS.Game.GameObjectId))
        if uGameObjectId ~= nil then
            uGameObjectId.ID = nObjectId
        end
    end
    CltGameMod.processEvent(Const.EventType.AddMonster)
end

function roleDead(nGameId, nObjectId)
    if not CltGameMod.checkGameId(nGameId) then
        return
    end
    local tCltGame = CltGameMod.getGame()
    local tGameObject = tCltGame.tGameObjects[nObjectId]
    if tGameObject == nil then
        return
    end
    if tGameObject.nObjectType == Const.GameObjectType.Monster then
        CltGameMod.processEvent(Const.EventType.MonsterDead, nObjectId)
    end
    
    CltAnimatorMod.roleDead(tGameObject, function()
        local goInstance = tGameObject.goInstance
        if goInstance ~= nil then
            UE.GameObject.Destroy(goInstance)
        end
    end)
    tCltGame.tGameObjects[nObjectId] = nil
end

function getMonsterByRefreshId(sRefreshId)
    local tCltGame = CltGameMod.getGame()
    if tCltGame == nil then
        return
    end
    for _, tGameObject in pairs(tCltGame.tGameObjects) do
        if tGameObject.nObjectType == Const.GameObjectType.Monster then
            if tGameObject.sRefreshId == sRefreshId then
                return tGameObject
            end
        end
    end
end