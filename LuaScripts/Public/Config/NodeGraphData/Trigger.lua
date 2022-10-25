Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Trigger = {
    tNodeMap = {
        [1] = {nNodeType = 0, nNodeId = 1},
        [2] = {nNodeType = 15, nTriggerId = 1, nNodeId = 2},
        [3] = {nNodeType = 1, sContext = "aaaa", bIsError = true, nNodeId = 3},
        [4] = {nNodeType = 15, nTriggerId = 2, nNodeId = 4},
        [5] = {nNodeType = 1, sContext = "nnnnn", bIsError = true, nNodeId = 5},
    },
    nStartNodeId = 1,
    tTransitions = {
        [1] = {
            [1] = {2, 4},
        },
        [2] = {
            [1] = {3},
        },
        [4] = {
            [1] = {5},
        },
    },
}