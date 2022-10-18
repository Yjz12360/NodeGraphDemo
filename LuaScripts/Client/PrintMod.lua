
function print(sContext)
    if type(sContext) ~= "string" then
        sContext = tostring(sContext)
    end
    if sContext == nil then return end
    CS.UnityEngine.Debug.Log(sContext)
end

function printError(sContext)
    if type(sContext) ~= "string" then
        sContext = tostring(sContext)
    end
    if sContext == nil then return end
    CS.UnityEngine.Debug.LogError(sContext)
end

function printErrorTrace(sContext)
    if type(sContext) ~= "string" then
        sContext = tostring(sContext)
    end
    if sContext == nil then return end
    CS.UnityEngine.Debug.LogError(sContext .. debug.traceback())
end

_G.print = print
_G.printError = printError
_G.printErrorTrace = printErrorTrace
