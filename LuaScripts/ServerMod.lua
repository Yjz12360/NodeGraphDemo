-- local ServerMod = {}
-- package.loaded["ServerMod"] = ServerMod

-- SvrGameMod = require "SvrGameMod"

-- local tSvrGames = {}

-- local nCurrGameId = 0
-- local function genGameId()
--     nCurrGameId = nCurrGameId + 1
--     return nCurrGameId
-- end

-- function recvCreateGame(nGameConfigId)
--     local nGameId = genGameId()
--     local tSvrGame = SvrGameMod.addGame(nGameId, nGameConfigId)
--     tSvrGames[nGameId] = tSvrGame
--     Messager.S2CCreateGameSucc(tSvrGame.nGameId)
--     SvrGameMod.initGame(tSvrGame)
-- end

-- function update(nDeltaTime)
--     for _, tSvrGame in pairs(tSvrGames) do
--         SvrGameMod.updateGame(nDeltaTime)
--     end
-- end

-- ServerMod.recvCreateGame = recvCreateGame
-- return ServerMod
