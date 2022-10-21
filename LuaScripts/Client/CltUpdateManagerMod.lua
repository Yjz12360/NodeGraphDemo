
local nCurrId = 0
local tHandlers = nil
local tUnregister = nil

function register(fUpdate)
    tHandlers = tHandlers or {}
    nCurrId = nCurrId + 1
    tHandlers[nCurrId] = fUpdate
    return nCurrId
end

function unregister(nUpdateId)
    tUnregister = tUnregister or {}
    tUnregister[nUpdateId] = true
end

function update(nDeltaTime)
    if tHandlers == nil then
        return
    end
    for _, fUpdate in pairs(tHandlers) do
        if fUpdate ~= nil then
            fUpdate(nDeltaTime)
        end
    end
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