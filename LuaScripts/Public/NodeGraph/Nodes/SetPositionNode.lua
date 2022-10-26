
function SvrHandler(tNodeGraph, tNodeData)
    local bSetPlayer = tNodeData.bSetPlayer
    local nRefreshId = tNodeData.nRefreshId

    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local nPosX, nPosY, nPosZ = 0, 0, 0
    local sPosId = tNodeData.sPosId
    if sPosId ~= nil and sPosId ~= "" then
        local tPos = GameSceneCfgMod.getPosition(tSvrGame.tGameSceneConfig, sPosId)
        if tPos ~= nil then
            nPosX, nPosY, nPosZ = tPos.x, tPos.y, tPos.z
        end
    else
        nPosX, nPosY, nPosZ = tNodeData.nPosX, tNodeData.nPosY, tNodeData.nPosZ
    end

    local tGameObject
    if bSetPlayer then
        local tGameObjects = SvrGameMod.getObjects(tSvrGame)
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Player then
                SvrGameRoleMod.forceSetPos(tSvrGame, tGameObject.nObjectId, nPosX, nPosY, nPosZ)
            end
        end
    else
        local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, nRefreshId)
        if tGameObject ~= nil then
            SvrGameRoleMod.forceSetPos(tSvrGame, tGameObject.nObjectId, nPosX, nPosY, nPosZ)
        end
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId, true)
end
