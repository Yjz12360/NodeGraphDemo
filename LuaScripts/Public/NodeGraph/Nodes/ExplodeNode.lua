
function CltHandler(tNodeGraph, tNodeData)
    local nExplosionId = tNodeData.nExplosionId
    local sPosId = tNodeData.sPosId
    local tCltGame = CltGameMod.getGame()
    local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sPosId)
    if tPos ~= nil then
        local tExplosionConfig = Config.Explosion[nExplosionId]
        if tExplosionConfig ~= nil then
            local nEffectId = tExplosionConfig.nEffectId
            CltEffectMod.playEffect(nEffectId, tPos.x, tPos.y, tPos.z)
        end
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nExplosionId = tNodeData.nExplosionId
    local sPosId = tNodeData.sPosId
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tPos = GameSceneCfgMod.getPosition(tSvrGame.tGameSceneConfig, sPosId)
    if tPos == nil then
        SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
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
                        SvrGameRoleMod.roleDead(tSvrGame, tGameObject.nObjectId)
                    end
                end
            end
        end
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end