
function CltHandler(tNodeGraph, tNodeData)
    local sRefreshId = tNodeData.sRefreshId
    local sSetTrigger = tNodeData.sSetTrigger

    local tGameObject = CltGameRoleMod.getMonsterByRefreshId(sRefreshId)
    if tGameObject ~= nil then
        local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
        if uAnimator ~= nil then
            uAnimator:SetTrigger(sSetTrigger)
        end
    end
    CltNodeGraphMod.finishNode(tNodeGraph, tNodeData.nNodeId)
end
