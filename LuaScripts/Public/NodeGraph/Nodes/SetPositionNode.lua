
function CltHandler(tNodeGraph, tNodeData)
    local bSetPlayer = tNodeData.bSetPlayer
    local sRefreshId = tNodeData.sRefreshId

    local tCltGame = CltGameMod.getGame()
    local nPosX, nPosY, nPosZ = 0, 0, 0
    local sPosId = tNodeData.sPosId
    if sPosId ~= nil and sPosId ~= "" then
        local tPos = GameSceneCfgMod.getPosition(tCltGame.tGameSceneConfig, sPosId)
        if tPos ~= nil then
            nPosX, nPosY, nPosZ = tPos.x, tPos.y, tPos.z
        end
    else
        nPosX, nPosY, nPosZ = tNodeData.nPosX, tNodeData.nPosY, tNodeData.nPosZ
    end

    local tGameObject
    if bSetPlayer then
        local tGameObjects = CltGameMod.getObjects()
        for _, tGameObject in pairs(tGameObjects) do
            if tGameObject.nObjectType == Const.GameObjectType.Player then
                local goInstance = tGameObject.goInstance
                if goInstance ~= nil then
                    goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
                end
            end
        end
        CltCameraMod.update()
    else
        local tGameObject = CltGameRoleMod.getMonsterByRefreshId(sRefreshId)
        if tGameObject ~= nil then
            local goInstance = tGameObject.goInstance
            if goInstance ~= nil then
                goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
            end
        end
    end

    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local bSetPlayer = tNodeData.bSetPlayer
    local sRefreshId = tNodeData.sRefreshId

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
                tGameObject.nPosX = nPosX
                tGameObject.nPosY = nPosY
                tGameObject.nPosZ = nPosZ
            end
        end
    else
        local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, sRefreshId)
        if tGameObject ~= nil then
            tGameObject.nPosX = nPosX
            tGameObject.nPosY = nPosY
            tGameObject.nPosZ = nPosZ
        end
    end

    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
