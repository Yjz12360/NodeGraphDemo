
local tMessages = nil
local function addMessage(fHandler, ...)
    local tMessage = {
        fHandler = fHandler,
        tArgs = {...},
    }
    tMessages = tMessages or {}
    table.insert(tMessages, tMessage)
end

function update()
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
S2CAddNodeGraph = function(nGameId, nNodeGraphId, nConfigId)
    addMessage(CltGameMod.addNodeGraph, nGameId, nNodeGraphId, nConfigId)
end

S2CFinishNode = function(nGameId, nNodeGraphId, sNodeId, nPath)
    addMessage(CltNodeGraphMod.recvFinishNode, nGameId, nNodeGraphId, sNodeId, nPath)
end

S2CFinishNodeGraph = function(nGameId, nNodeGraphId)
    addMessage(CltNodeGraphMod.recvFinishNodeGraph, nGameId, nNodeGraphId)
end


-- Game
C2SCreateGame = function(nGameConfigId)
    addMessage(SvrGameMod.addGame, nGameConfigId)
end

S2CCreateGameSucc = function(nGameId)
    addMessage(CltGameMod.recvCreateGameSucc, nGameId)
end

S2CAddPlayer = function(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
    addMessage(CltGameMod.addPlayer, nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
end

C2SEnterTrigger = function(nGameId, nTriggerId)
    addMessage(SvrGameMod.onEnterTrigger, nGameId, nTriggerId)
end