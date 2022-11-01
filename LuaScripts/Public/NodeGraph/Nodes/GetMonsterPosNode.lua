
function CltHandler(tNodeGraph, tNodeData)
    local nNodeId = tNodeData.nNodeId
    local nRefreshId = tNodeData.nRefreshId
    local tCltGame = CltGameMod.getGame()
    local tPos = {x = 0, y = 0, z = 0}
    local tGameObject = CltGameRoleMod.getMonsterByRefreshId(nRefreshId)
    if tGameObject ~= nil then
        local goInstance = tGameObject.goInstance
        if goInstance ~= nil then
            local vecPos = goInstance.transform.position
            tPos.x = vecPos.x
            tPos.y = vecPos.y
            tPos.z = vecPos.z
        end
    end
    CltNodeGraphMod.setNodeOutput(tNodeGraph, nNodeId, "tPos", tPos)
    CltNodeGraphMod.finishNode(tNodeGraph, nNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nNodeId = tNodeData.nNodeId
    local nRefreshId = tNodeData.nRefreshId
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tGameObject = SvrGameRoleMod.getMonsterByRefreshId(tSvrGame, nRefreshId)
    if tGameObject ~= nil then
        local tPos = {}
        tPos.x = tGameObject.nPosX
        tPos.y = tGameObject.nPosY
        tPos.z = tGameObject.nPosZ
        SvrNodeGraphMod.setNodeOutput(tNodeGraph, nNodeId, "tPos", tPos)
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, nNodeId)
end
