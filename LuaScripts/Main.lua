require "Config/Game"
require "Config/Model"
require "Config/Monster"
require "Config/NodeGraph"
require "Config/Player"

require "PrintMod"
CltGameMod = require "CltGameMod"
SvrGameMod = require "SvrGameMod"
Messager = require "Messager"

function Init()
    require("LuaPanda").start("127.0.0.1", 8818)

    UE = CS.UnityEngine


    CltGameMod.init()
end

function Update(nDeltaTime)
    Messager.update()

    if CltGameMod.isLocalGame() then
        SvrGameMod.update(nDeltaTime)
    end
    CltGameMod.update(nDeltaTime)

    -- ServerMod.update(nDeltaTime)
end

Init()