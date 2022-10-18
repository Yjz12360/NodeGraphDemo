Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua4 = {
    tNodeMap = {
        ["3"] = {
            nDelayTime = 1.0,
            sNodeId = "3",
            nNodeType = 3,
        },
        ["4"] = {
            sNodeId = "4",
            bIsError = true,
            sContext = "Print 2",
            nNodeType = 1,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["2"] = {
            sNodeId = "2",
            bIsError = true,
            sContext = "Print 1",
            nNodeType = 1,
        },
        ["5"] = {
            sNodeId = "5",
            bIsError = false,
            sContext = "Print 5",
            nNodeType = 1,
        },
    },
    sStartNodeId = "1",
    tTransitions = {
        ["3"] = {
            sFromNodeId = "2",
            sToNodeId = "3",
            nPath = 1,
        },
        ["0"] = {
            sFromNodeId = "1",
            sToNodeId = "2",
            nPath = 1,
        },
        ["1"] = {
            sFromNodeId = "4",
            sToNodeId = "5",
            nPath = 1,
        },
        ["2"] = {
            sFromNodeId = "3",
            sToNodeId = "4",
            nPath = 1,
        },
    },
}