
function SvrHandler(tNodeGraph, tNodeData)
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    if tSvrGame == nil then
        return
    end
    local tAIManager = tSvrGame.tAIManager
    local sRefreshId = tNodeData.sRefreshId
    local sGroupId = tNodeData.sGroupId
    local bActive = tNodeData.bActive
    if sRefreshId and sRefreshId ~= "" then
        local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, sRefreshId)
        if tGameObject ~= nil then
            AIManagerMod.setAIActive(tAIManager, tGameObject.nObjectId, bActive)
        end
    end
    if sGroupId and sGroupId ~= nil then
        local tGameObjects = SvrGameMod.getObjects(tSvrGame)
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Monster then
                if tGameObject.sGroupId == sGroupId then
                    AIManagerMod.setAIActive(tAIManager, tGameObject.nObjectType, bActive)
                end
            end
        end
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end
