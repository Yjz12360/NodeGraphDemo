
local nCurrId = 0
local tHandlers = nil
local tUnregister = nil
local tRegMods = {}
local bUpdate = false

function register(fUpdate)
    tHandlers = tHandlers or {}
    nCurrId = nCurrId + 1
    tHandlers[nCurrId] = fUpdate
    return nCurrId
end

function registerMod(sMod)
    if tRegMods[sMod] then
        printError("registerMod error: mod already registered: " .. sMod)
        return
    end
    local tMod = _G[sMod]
    if tMod == nil then
        printError("registerMod error: mod not found: " .. sMod)
        return
    end
    if tMod.update ~= nil then
        local nUpdateId = CltUpdateManagerMod.register(function(nDeltaTime)
            tMod.update(nDeltaTime)
        end)
        tRegMods[sMod] = nUpdateId
        return nUpdateId
    end
end

function unregister(nUpdateId)
    if bUpdate then
        tUnregister = tUnregister or {}
        tUnregister[nUpdateId] = true
    else
        tHandlers[nUpdateId] = nil
        if TableUtil.isEmpty(tHandlers) then
            tHandlers = nil
        end
    end
end

function unregisterMod(sMod)
    local nUpdateId = tRegMods[sMod]
    if nUpdateId == nil then
        printError("unregisterMod error: mod didn't registered: " .. sMod)
        return
    end
    CltUpdateManagerMod.unregister(nUpdateId)
    tRegMods[sMod] = nil
end

function checkRegisterMod(sMod)
    return tRegMods[sMod] ~= nil
end

function update(nDeltaTime)
    if tHandlers == nil then
        return
    end
    bUpdate = true
    for nUpdateId, fUpdate in pairs(tHandlers) do
        if fUpdate ~= nil then
            fUpdate(nDeltaTime)
        end
    end
    bUpdate = false
    if tUnregister ~= nil then
        for nUpdateId, _ in pairs(tUnregister) do
            tHandlers[nUpdateId] = nil
        end
        tUnregister = nil
        if TableUtil.isEmpty(tHandlers) then
            tHandlers = nil
        end
    end
end