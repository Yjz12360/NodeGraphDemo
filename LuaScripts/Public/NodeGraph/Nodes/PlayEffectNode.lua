
function CltHandler(tNodeGraph, tNodeData)
    local nEffectId = tNodeData.nEffectId
    local sPosId = tNodeData.sPosId
    local tCltGame = CltGameMod.getGame()
    local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sPosId)
    if tPos == nil then
        CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
        return
    end
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
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

