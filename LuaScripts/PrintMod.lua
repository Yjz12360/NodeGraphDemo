local PrintMod = {}
package.loaded["PrintMod"] = PrintMod

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

PrintMod.print = print
PrintMod.printError = printError
PrintMod.printErrorTrace = printErrorTrace
return PrintMod