
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if tSvrGame == nil then
        return
    end
    local tAIManager = tSvrGame.tAIManager
    local nRefreshId = tNodeData.nRefreshId
    local nGroupId = tNodeData.nGroupId
    local bActive = tNodeData.bActive
    if nRefreshId and nRefreshId ~= "" then
        local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, nRefreshId)
        if tGameObject ~= nil then
            AIManagerMod.setAIActive(tAIManager, tGameObject.nObjectId, bActive)
        end
    end
    if nGroupId and nGroupId ~= nil then
        local tGameObjects = SvrGameMod.getObjects(tSvrGame)
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Monster then
                if tGameObject.nGroupId == nGroupId then
                    AIManagerMod.setAIActive(tAIManager, tGameObject.nObjectType, bActive)
                end
            end
        end
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end
