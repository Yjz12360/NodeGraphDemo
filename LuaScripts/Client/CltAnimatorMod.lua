
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

function startMove(tGameObject)
    local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
    if uAnimator == nil then
        return
    end
    uAnimator:SetTrigger("Move")
end

function endMove(tGameObject)
    local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
    if uAnimator == nil then
        return
    end
    uAnimator:SetTrigger("EndMove")
end

function doPreform(tGameObject)
    local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
    if uAnimator == nil then
        return
    end
    uAnimator:SetTrigger("Preform")
end