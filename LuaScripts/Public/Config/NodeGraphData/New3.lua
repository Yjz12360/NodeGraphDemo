Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.New3 = {
    tTransitions = {
        [1] = {
            [1] = {"2", "3", "4", "5"},
        },
        [2] = {
            [1] = {"6"},
        },
    },
    nStartNodeId = "1",
    tNodeMap = {
        [1] = {nNodeType = 0, nNodeId = "1"},
        [2] = {sContext = "2", nNodeType = 1, bIsError = false, nNodeId = "2"},
        [3] = {sContext = "3", nNodeType = 1, bIsError = false, nNodeId = "3"},
        [4] = {sContext = "4", nNodeType = 1, bIsError = false, nNodeId = "4"},
        [5] = {sContext = "5", nNodeType = 1, bIsError = false, nNodeId = "5"},
        [6] = {sContext = "6", nNodeType = 1, bIsError = false, nNodeId = "6"},
    },
}