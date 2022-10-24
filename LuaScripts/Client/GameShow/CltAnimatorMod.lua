
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
    CltAnimatorMod.setTrigger(tGameObject, "Move")
end

function endMove(tGameObject)
    CltAnimatorMod.setTrigger(tGameObject, "EndMove")
end

function roleDead(tGameObject, fCallback)
    local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
    if uAnimator ~= nil then
        uAnimator:SetTrigger("Die")
        TimerMod.delay(2, function()
            if fCallback ~= nil then
                fCallback()
            end
        end)
    else
        if fCallback ~= nil then
            fCallback()
        end
    end
end

function setTrigger(tGameObject, sTrigger)
    local uAnimator = CltAnimatorMod.getAnimator(tGameObject)
    if uAnimator == nil then
        return
    end
    uAnimator:SetTrigger(sTrigger)
end