
function playEffect(nEffectId, nPosX, nPosY, nPosZ)
    local tEffectConfig = Config.Effect[nEffectId]
    if tEffectConfig == nil then
        printError("playEffect error: config not exist: " .. tostring(nEffectId))
        return
    end
    sPrefabFile = tEffectConfig.sPrefabFile
    if sPrefabFile == nil or sPrefabFile == "" then
        printError("playEffect error: prefab file not exist: " .. sPrefabFile)
        return
    end
    local goPrefab = UE.Resources.Load(sPrefabFile)
    if goPrefab == nil then
        printError("playEffect error: prefab load fail: " .. sPrefabFile)
        return
    end
    local goEffect = UE.GameObject.Instantiate(goPrefab)
    goEffect.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
    local nDuration = tEffectConfig.nDuration
    if nDuration ~= nil and nDuration > 0 then
        TimerMod.delay(nDuration, function()
            if goEffect ~= nil then
                UE.GameObject.Destroy(goEffect)
            end
        end)
    end
end