require("Tools/LuaPanda").start("127.0.0.1", 8818)

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
rawRequire("Public/PubHeader")
rawRequire("Server/SvrHeader")
rawRequire("Client/CltHeader")
_G.require = _G.rawRequire

function Init()

    UE = CS.UnityEngine


    CltGameMod.init()
end

function Update(nDeltaTime)
    Messager.update()

    TimerMod.update(nDeltaTime)

    CltAIActionMod.update(nDeltaTime)
    CltLocalPlayerMod.update(nDeltaTime)

    -- if CltGameMod.isLocalGame() then
    --     SvrGameMod.update(nDeltaTime)
    -- end
    -- CltGameMod.update(nDeltaTime)
end

function OnTriggerEnter(nTriggerId, uCollider)
    CltGameMod.onTriggerEnter(nTriggerId, uCollider)
end

function OnAttackHit(nAttackerId, nTargetId)
    CltGameMod.onAttackHit(nAttackerId, nTargetId)
end

Init()