Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua5 = {
    tTransitions = {
        ["3"] = {
            sToNodeId = "5",
            nPath = 1,
            sFromNodeId = "1",
        },
        ["4"] = {
            sToNodeId = "6",
            nPath = 1,
            sFromNodeId = "5",
        },
        ["1"] = {
            sToNodeId = "4",
            nPath = 1,
            sFromNodeId = "3",
        },
        ["2"] = {
            sToNodeId = "3",
            nPath = 1,
            sFromNodeId = "2",
        },
        ["0"] = {
            sToNodeId = "2",
            nPath = 1,
            sFromNodeId = "1",
        },
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["5"] = {
            nTriggerId = 1,
            nNodeType = 15,
            sNodeId = "5",
        },
        ["6"] = {
            sNodeId = "6",
            bIsError = true,
            nNodeType = 1,
            sContext = "WaitTriggerTest",
        },
        ["3"] = {
            nDelayTime = 1.0,
            nNodeType = 3,
            sNodeId = "3",
        },
        ["4"] = {
            sNodeId = "4",
            bIsError = true,
            nNodeType = 1,
            sContext = "Print 2",
        },
        ["1"] = {
            nNodeType = 0,
            sNodeId = "1",
        },
        ["2"] = {
            sNodeId = "2",
            bIsError = true,
            nNodeType = 1,
            sContext = "Print 1",
        },
    },
}