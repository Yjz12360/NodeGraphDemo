
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nGroupId = tNodeData.nGroupId

    local nRefreshInv = tNodeData.nRefreshInv or 2
    if tNodeData.bInfinite then
        SvrGameRoleMod.refreshMonsterGroup(tSvrGame, nGroupId)
        local nRefreshTimer
        nRefreshTimer = TimerMod.add(nRefreshInv, function()
            local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
            if tSvrGame == nil then
                TimerMod.remove(nRefreshTimer)
                return
            end
            SvrGameRoleMod.refreshMonsterGroup(tSvrGame, nGroupId)
        end)
    else
        SvrGameRoleMod.refreshMonsterGroup(tSvrGame, nGroupId)
        local nRefreshCount = tNodeData.nRefreshCount or 1
        if nRefreshCount > 1 then
            local nCurrCount = 1
            local nRefreshTimer
            nRefreshTimer = TimerMod.add(nRefreshInv, function()
                local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
                if tSvrGame == nil then
                    TimerMod.remove(nRefreshTimer)
                    return
                end
                SvrGameRoleMod.refreshMonsterGroup(tSvrGame, nGroupId)
                nCurrCount = nCurrCount + 1
                if nCurrCount >= nRefreshCount then
                    TimerMod.remove(nRefreshTimer)
                    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true)
                    return
                end
            end)
        else
            SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true)
        end
    end
end
