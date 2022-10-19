
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
        local uMonsterControl = goInstance:GetComponent(typeof(CS.Game.MonsterControl))
        if uMonsterControl ~= nil then
            uAnimator = uMonsterControl.modelAnimator
            tGameObject.uAnimator = uAnimator
        end
    end
    return uAnimator
end