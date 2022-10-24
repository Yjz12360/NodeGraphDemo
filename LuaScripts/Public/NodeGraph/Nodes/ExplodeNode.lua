
function CltHandler(tNodeGraph, tNodeData)
    local nExplosionId = tNodeData.nExplosionId
    local sPosId = tNodeData.sPosId
    local tCltGame = CltGameMod.getGame()
    local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sPosId)
    if tPos == nil then
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        return
    end
    local tExplosionConfig = Config.Explosion[nExplosionId]
    if tExplosionConfig ~= nil then
        local nEffectId = tExplosionConfig.nEffectId
        local tEffectConfig = Config.Effect[nEffectId]
        if tEffectConfig ~= nil then
            sPrefabFile = tEffectConfig.sPrefabFile
            if sPrefabFile and sPrefabFile ~= "" then
                local goPrefab = UE.Resources.Load(sPrefabFile)
                if goPrefab ~= nil then
                    local goEffect = UE.GameObject.Instantiate(goPrefab)
                    local tCltGame = CltGameMod.getGame()
                    goEffect.transform.position = UE.Vector3(tPos.x, tPos.y, tPos.z)
                    local nDuration = tEffectConfig.nDuration
                    if nDuration ~= nil and nDuration > 0 then
                        TimerMod.delay(nDuration, function()
                            if goEffect ~= nil then
                                UE.GameObject.Destroy(goEffect)
                            end
                        end)
                    end
                end
            end
        end
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nExplosionId = tNodeData.nExplosionId
    local sPosId = tNodeData.sPosId
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tPos = GameSceneCfgMod.getPosition(tSvrGame.tGameSceneConfig, sPosId)
    if tPos == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        return
    end
    local tExplosionConfig = Config.Explosion[nExplosionId]
    if tExplosionConfig ~= nil then
        local nRadius = tExplosionConfig.nRadius
        local nSqrRadius = nRadius * nRadius
        local nPosX, nPosY, nPosZ = tPos.x, tPos.y, tPos.z
        local nDamage = tExplosionConfig.nDamage
        local tGameObjects = SvrGameMod.getObjects(tSvrGame)
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Monster or
                tGameObject.nObjectType == Const.GameObjectType.Player then

                local nTarX, nTarY, nTarZ = tGameObject.nPosX, tGameObject.nPosY, tGameObject.nPosZ
                local nSqrDistance = VectorUtil.sqrMagnitude(nPosX, nPosY, nPosZ, nTarX, nTarY, nTarZ)
                if nSqrDistance <= nSqrRadius then
                    tGameObject.nCurrHP = tGameObject.nCurrHP - nDamage
                    if tGameObject.nCurrHP <= 0 then
                        tGameObject.nCurrHP = 0
                        SvrGameMod.roleDead(tSvrGame, tGameObject.nObjectId)
                    end
                end
            end
        end
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end