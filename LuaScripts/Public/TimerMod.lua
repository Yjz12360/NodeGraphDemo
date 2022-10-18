-- 暂时使用Update来模拟定时器功能，后面优化
local tTimers = {}
local nCurrId = 1

function delay(nTime, fHandler)
    local tTimer = {
        nCurrId = nCurrId,
        nTime = nTime,
        nCurrTime = 0,
        fHandler = fHandler,
    }
    nCurrId = nCurrId + 1
    tTimers[nCurrId] = tTimer
end

local tToDel = {}
function update(nDeltaTime)
    for nCurrId, tTimer in pairs(tTimers) do
        tTimer.nCurrTime = tTimer.nCurrTime + nDeltaTime
        if tTimer.nCurrTime >= tTimer.nTime then
            local fHandler = tTimer.fHandler
            if fHandler ~= nil then
                fHandler()
                table.insert(tToDel, nCurrId)
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