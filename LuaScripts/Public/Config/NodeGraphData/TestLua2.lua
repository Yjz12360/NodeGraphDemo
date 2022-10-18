Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua2 = {
    tTransitions = {
        ["3"] = {
            nPath = 1,
            sToNodeId = "5",
            sFromNodeId = "4",
        },
        ["2"] = {
            nPath = 1,
            sToNodeId = "4",
            sFromNodeId = "3",
        },
        ["1"] = {
            nPath = 1,
            sToNodeId = "3",
            sFromNodeId = "2",
        },
        ["0"] = {
            nPath = 1,
            sToNodeId = "2",
            sFromNodeId = "1",
        },
    },
    tNodeMap = {
        ["3"] = {
            sNodeId = "3",
            nDelayTime = 1.0,
            nNodeType = 3,
        },
        ["2"] = {
            sNodeId = "2",
            bIsError = true,
            sContext = "Print 1",
            nNodeType = 1,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["5"] = {
            sNodeId = "5",
            bIsError = false,
            sContext = "Print 5",
            nNodeType = 1,
        },
        ["4"] = {
            sNodeId = "4",
            bIsError = true,
            sContext = "Print 2",
            nNodeType = 1,
        },
    },
    sStartNodeId = "1",
}