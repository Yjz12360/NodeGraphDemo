Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Random = {
    tNodeMap = {
        ["1"] = {nNodeType = 0, sNodeId = "1"},
        ["4"] = {sNodeId = "4", bIsError = true, sContext = "bbbbb", nNodeType = 1},
        ["3"] = {sNodeId = "3", bIsError = true, sContext = "aaaaa", nNodeType = 1},
        ["2"] = {nNodeType = 17, sNodeId = "2"},
    },
    tTransitions = {
        ["1"] = {sToNodeId = "3", nPath = 1, sFromNodeId = "2"},
        ["0"] = {sToNodeId = "2", nPath = 1, sFromNodeId = "1"},
        ["2"] = {sToNodeId = "4", nPath = 2, sFromNodeId = "2"},
    },
    sStartNodeId = "1",
}