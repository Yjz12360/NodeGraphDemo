local Messager = {}
package.loaded["Messager"] = Messager

PrintMod = require "PrintMod"
ServerMod = require "ServerMod"
CltGameMod = require "CltGameMod"
SvrGameMod = require "SvrGameMod"
CltNodeGraphMod = require "CltNodeGraphMod"
SvrNodeGraphMod = require "SvrNodeGraphMod"
TableUtil = require "TableUtil"

local tMessages = nil
local function addMessage(fHandler, ...)
    local tMessage = {
        fHandler = fHandler,
        tArgs = {...},
    }
    tMessages = tMessages or {}
    table.insert(tMessages, tMessage)
end

function Messager.update()
    if tMessages ~= nil and #tMessages > 0 then
        for _, tMessage in ipairs(tMessages) do
            local fHandler = tMessage.fHandler
            if fHandler ~= nil then
                local tArgs = tMessage.tArgs
                if tArgs ~= nil then
                    fHandler(table.unpack(tArgs))
                else
                    fHandler()
                end
            end
        end
        tMessages = nil
        -- TableUtil.clearArray(tMessages)
    end
end



-- NodeGraph
Messager.S2CAddNodeGraph = function(nGameId, nNodeGraphId, nConfigId)
    addMessage(CltGameMod.addNodeGraph, nGameId, nNodeGraphId, nConfigId)
end

Messager.S2CFinishNode = function(nNodeGraphId, sNodeId, nPath)
    addMessage(CltNodeGraphMod.recvFinishNode, sNodeId, nPath)
end


-- Game
Messager.C2SCreateGame = function(nGameConfigId)
    addMessage(SvrGameMod.addGame, nGameConfigId)
end

Messager.S2CCreateGameSucc = function(nGameId)
    addMessage(CltGameMod.recvCreateGameSucc, nGameId)
end

Messager.S2CAddPlayer = function(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
    addMessage(CltGameMod.addPlayer, nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
end

return Messager