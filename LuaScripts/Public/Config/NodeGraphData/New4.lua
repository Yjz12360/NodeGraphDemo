Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.New4 = {
    tTransitions = {
        [1] = {
            [1] = {4, 3, 2, 5},
        },
        [2] = {
            [1] = {8, 7, 6},
        },
    },
    tNodeMap = {
        [1] = {nNodeId = 1, nNodeType = 0},
        [2] = {bIsError = true, nNodeType = 1, nNodeId = 2, sContext = "2"},
        [3] = {bIsError = true, nNodeType = 1, nNodeId = 3, sContext = "3"},
        [4] = {bIsError = true, nNodeType = 1, nNodeId = 4, sContext = "4"},
        [5] = {bIsError = true, nNodeType = 1, nNodeId = 5, sContext = "5"},
        [6] = {bIsError = true, nNodeType = 1, nNodeId = 6, sContext = "6"},
        [7] = {bIsError = true, nNodeType = 1, nNodeId = 7, sContext = "7"},
        [8] = {bIsError = true, nNodeType = 1, nNodeId = 8, sContext = "aaaaa"},
    },
    nStartNodeId = 1,
}