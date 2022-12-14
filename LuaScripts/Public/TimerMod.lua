-- 暂时使用Update来模拟定时器功能，后面优化
local tTimers = {}
local nCurrId = 1

function add(nTime, fHandler)
    if type(nTime) ~= "number" or nTime <= 0 then
        printError("TimerMod add error: nTime invalid: " .. tostring(nTime))
        return
    end
    local tTimer = {
        nCurrId = nCurrId,
        nTime = nTime,
        nCurrTime = 0,
        fHandler = fHandler,
        bLoop = true,
    }
    nCurrId = nCurrId + 1
    tTimers[nCurrId] = tTimer
    return nCurrId
end

function delay(nTime, fHandler)
    if type(nTime) ~= "number" or nTime <= 0 then
        printError("TimerMod delay error: nTime invalid: " .. tostring(nTime))
        return
    end
    local tTimer = {
        nCurrId = nCurrId,
        nTime = nTime,
        nCurrTime = 0,
        fHandler = fHandler,
        bLoop = false,
    }
    nCurrId = nCurrId + 1
    tTimers[nCurrId] = tTimer
    return nCurrId
end

function remove(nTimerId)
    tTimers[nTimerId] = nil
end

local tToDel = {}
function update(nDeltaTime)
    for nCurrId, tTimer in pairs(tTimers) do
        tTimer.nCurrTime = tTimer.nCurrTime + nDeltaTime
        if tTimer.nCurrTime >= tTimer.nTime then
            local fHandler = tTimer.fHandler
            if fHandler ~= nil then
                fHandler()
                if tTimer.bLoop then
                    tTimer.nCurrTime = 0
                else
                    table.insert(tToDel, nCurrId)
                end
                
            end
        end
    end
    if not TableUtil.isEmpty(tToDel) then
        for _, nCurrId in ipairs(tToDel) do
            tTimers[nCurrId] = nil
        end
        TableUtil.clearArray(tToDel)
    end
end