
function getAnimator(tGameObject)
    if tGameObject == nil then
        return
    end
    if tGameObject.nObjectType ~= Const.GameObjectType.Player and
        tGameObject.nObjectType ~= Const.GameObjectType.Monster then
        return
    end
    local goInstance = tGameObject.goInstance
    if goInstance == nil then
        return
    end

    local uAnimator = tGameObject.uAnimator
    if uAnimator == nil then
        uAnimator = goInstance:GetComponentInChildren(typeof(UE.Animator))
    end
    return uAnimator
end