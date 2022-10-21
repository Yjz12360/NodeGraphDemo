
function CltHandler(tNodeGraph, tNodeData)
    local nObjectId = tNodeData.nObjectId
    local tCltGame = CltGameMod.getGame()
    local tGameObject = CltGameMod.getObject(nObjectId)
    if tGameObject ~= nil then
        local goInstance = tGameObject.goInstance
        if goInstance ~= nil then
            local nPosX, nPosY, nPosZ
            local sPosId = tNodeData.sPosId
            if sPosId ~= nil and sPosId ~= "" then
                nPosX, nPosY, nPosZ = 0, 0, 0 -- TODO
            else
                nPosX, nPosY, nPosZ = tNodeData.nPosX, tNodeData.nPosY, tNodeData.nPosZ
            end
            goInstance.transform.position = UE.Vector3(nPosX, nPosY, nPosZ)
        end
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end

function SvrHandler(tNodeGraph, tNodeData)
    local nObjectId = tNodeData.nObjectId
    local tSvrGame = SvrGameMod.getGameById(tNodeGraph.nGameId)
    local tGameObject = SvrGameMod.tSvrGame(nObjectId)
    if tGameObject ~= nil then
        local nPosX, nPosY, nPosZ
        local sPosId = tNodeData.sPosId
        if sPosId ~= nil and sPosId ~= "" then
            nPosX, nPosY, nPosZ = 0, 0, 0 -- TODO
        else
            nPosX, nPosY, nPosZ = tNodeData.nPosX, tNodeData.nPosY, tNodeData.nPosZ
        end
        tGameObject.nPosX = nPosX
        tGameObject.nPosY = nPosY
        tGameObject.nPosZ = nPosZ
    end
    SvrNodeGraphMod.finishNode(tNodeGraph, tNodeData.sNodeId)
end
