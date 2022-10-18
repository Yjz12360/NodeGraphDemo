
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
    local tAI = AIMod.addAI(tAIManager.nGameId, nObjectId)
    tAIManager.tAIInstances[nObjectId] = tAI
    return tAI
end

function delAI(tAIManager, nObjectId)
    if tAIManager == nil then
        return
    end
    tAIManager.tAIInstances[nObjectId] = nil
end

function updateAI(tAIManager, nDeltaTime)
    for _, tAI in pairs(tAIManager.tAIInstances) do
        AIMod.update(tAI, nDeltaTime)
    end
end 