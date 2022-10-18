
function addAIManager(tSvrGame)
    local tAIManager = {}
    tAIManager.nGameId = tSvrGame.nGameId
    tAIManager.tAIInstances = {}

    return tAIManager
end

function addAI(tAIManager, nObjectId)
    if tAIManager == nil then
        return
    end
    local tMonsterAI = AIMod.addAI(tAIManager.nGameId, nObjectId)
    tAIManager.tAIInstances[nObjectId] = tMonsterAI
    return tMonsterAI
end

function delAI(tAIManager, nObjectId)
    if tAIManager == nil then
        return
    end
    tAIManager[nObjectId] = nil
end

function updateAI(tAIManager, nDeltaTime)
    for _, tMonsterAI in pairs(tAIManager.tAIInstances) do
        AIMod.update(tMonsterAI, nDeltaTime)
    end
end 