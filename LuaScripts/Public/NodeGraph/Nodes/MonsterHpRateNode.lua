
function SvrHandler(tNodeGraph, tNodeData)
    local nRefreshId = tNodeData.nRefreshId
    local tHpRates = tNodeData.tHpRates
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, nRefreshId)
    local nPath = 1
    if tGameObject ~= nil then
        local nCurrHpRate = tGameObject.nCurrHP / tGameObject.tConfig.nHP
        for _, nRate in ipairs(tHpRates) do
            if nCurrHpRate >= nRate then
                break
            end
            nPath = nPath + 1
        end
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true, nPath)
end
