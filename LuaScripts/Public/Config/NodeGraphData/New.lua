Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.New = {
    sStartNodeId = "1",
    tTransitions = {
        ["2"] = {
            {sToNodeId = "3", nPath = 1},
        },
        ["1"] = {
            {sToNodeId = "2", nPath = 1},
        },
    },
    tNodeMap = {
        ["2"] = {sNodeId = "2", nNodeType = 1, sContext = "222", bIsError = true},
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["3"] = {sNodeId = "3", nNodeType = 1, sContext = "111", bIsError = false},
    },
}