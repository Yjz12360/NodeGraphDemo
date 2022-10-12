Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua3 = {
    tTransitions = {
        ["0"] = {
            nPath = 1,
            sFromNodeId = "3",
            sToNodeId = "4",
        },
        ["3"] = {
            nPath = 1,
            sFromNodeId = "4",
            sToNodeId = "5",
        },
        ["2"] = {
            nPath = 1,
            sFromNodeId = "1",
            sToNodeId = "2",
        },
        ["1"] = {
            nPath = 1,
            sFromNodeId = "2",
            sToNodeId = "3",
        },
    },
    tNodeMap = {
        ["2"] = {
            sNodeId = "2",
            nNodeType = 1,
            sContext = "Print 1",
            bIsError = true,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["4"] = {
            sNodeId = "4",
            nNodeType = 1,
            sContext = "Print 2",
            bIsError = true,
        },
        ["3"] = {
            sNodeId = "3",
            nNodeType = 3,
            nDelayTime = 1.0,
        },
        ["5"] = {
            sNodeId = "5",
            nNodeType = 1,
            sContext = "Print 5",
            bIsError = false,
        },
    },
    sStartNodeId = "1",
}