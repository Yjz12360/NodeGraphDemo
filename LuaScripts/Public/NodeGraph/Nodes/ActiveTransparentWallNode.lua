local transWallContainer = nil

function CltHandler(tNodeGraph, tNodeData)
    local sWallId = tNodeData.sWallId
    local bActive = tNodeData.bActive
    if sWallId ~= nil and sWallId ~= "" then
        if transWallContainer == nil then
            local goContainer = UE.GameObject.Find("SceneData/TransparentWall")
            if goContainer ~= nil then
                transWallContainer = goContainer.transform
            end
        end
        if transWallContainer ~= nil then
            local transWall = transWallContainer:Find(sWallId)
            if transWall ~= nil then
                transWall.gameObject:SetActive(bActive)
            end
        end
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end
