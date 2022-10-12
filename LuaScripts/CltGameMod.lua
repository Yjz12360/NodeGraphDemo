local CltGameMod = {}
package.loaded["CltGameMod"] = CltGameMod

require "PrintMod"
Messager = require "Messager"

local GameObjectType = {
    Player = 1,
    Monster = 2,
}

local nCurrGameId = 0
local tCltGame = nil

function init()
    local nGameConfigId = 1
    Messager.C2SCreateGame(nGameConfigId)
end

function recvCreateGameSucc(nGameId)
    nCurrGameId = nGameId
    CltGameMod.createGame()
end

function createGame()
    tCltGame = {}
    tCltGame.tGameObjects = {}
    tCltGame.tNodeGraphs = {}
end

function isLocalGame()
    return true
end

function addNodeGraph(nGameId, nNodeGraphId, nConfigId)
    if nGameId ~= nCurrGameId then
        return
    end
    local tNodeGraph = CltNodeGraphMod.addNodeGraph(tCltGame, nNodeGraphId, nConfigId)
    tCltGame.tNodeGraphs[nNodeGraphId] = tNodeGraph
    CltNodeGraphMod.startNodeGraph(tNodeGraph)
end

function addPlayer(nGameId, nObjectId, nConfigId, nPosX, nPosY, nPosZ)
    if nGameId ~= nCurrGameId then
        return
    end
    local tConfig = Config.Player[nConfigId]
    if tConfig == nil then
        printError("PlayerConfig not exist configId: " .. nConfigId)
        return
    end
    local tPlayer = {}
    tPlayer.nObjectId = nObjectId
    tPlayer.nObjectType = GameObjectType.Player
    tPlayer.tConfig = tConfig
    tPlayer.tPos = {
        x = nPosX,
        y = nPosY,
        z = nPosZ,
    }
    local nModelId = tConfig.nModelId
    local tModelConfig = Config.Model[nModelId]
    if tModelConfig ~= nil then
        local sPrefabFile = tModelConfig.sPrefabFile
        local goPrefab = UE.Resources.Load(sPrefabFile)
        local goInstance = UE.GameObject.Instantiate(goPrefab)
        tPlayer.goInstance = goInstance
    end
end

function update(nDeltaTime)
    if tCltGame == nil then
        return
    end
    for _, tNodeGraph in pairs(tCltGame.tNodeGraphs) do
        CltNodeGraphMod.updateNodeGraph(tNodeGraph, nDeltaTime)
    end
end

-- function monsterMove(nObjectId, nPosX, nPosY, nPosZ)

-- end

-- function monsterChase(nMonsterId, nTargetId, nChaseTime, nStopDistance)

-- end


-- function removeObject(nObjectId)

-- end


CltGameMod.init = init
CltGameMod.recvCreateGameSucc = recvCreateGameSucc
CltGameMod.createGame = createGame
CltGameMod.isLocalGame = isLocalGame
CltGameMod.addNodeGraph = addNodeGraph
CltGameMod.addPlayer = addPlayer
CltGameMod.update = update

return CltGameMod

-- return {
--     Init = Init,
-- }