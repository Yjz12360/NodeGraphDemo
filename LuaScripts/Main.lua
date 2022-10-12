require("LuaPanda").start("127.0.0.1", 8818)

require "ConfigHeader"
require "PrintMod"

_G.rawRequire = _G.require
_G.require = function(sModulePath)
    local sModuleName = string.match(sModulePath, ".+/(.+)") or sModulePath
    _G[sModuleName] = {}
    setmetatable(_G, {
        __newindex = function(table, key, value)
            table[sModuleName][key] = value
        end,
        __index = function(table, key)
            return table[sModuleName][key]
        end
    })
    _G.rawRequire(sModulePath)
    setmetatable(_G, nil)
end
rawRequire("Header")
_G.require = _G.rawRequire

function Init()

    UE = CS.UnityEngine

    CltGameMod.init()
end

function Update(nDeltaTime)
    Messager.update()

    if CltGameMod.isLocalGame() then
        SvrGameMod.update(nDeltaTime)
    end
    CltGameMod.update(nDeltaTime)
end

Init()