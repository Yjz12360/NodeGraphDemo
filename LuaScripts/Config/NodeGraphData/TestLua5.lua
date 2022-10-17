Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua5 = {
    tNodeMap = {
        ["2"] = {
            bIsError = true,
            sContext = "Print 1",
            sNodeId = "2",
            nNodeType = 1,
        },
        ["3"] = {
            nDelayTime = 1.0,
            sNodeId = "3",
            nNodeType = 3,
        },
        ["4"] = {
            bIsError = true,
            sContext = "Print 2",
            sNodeId = "4",
            nNodeType = 1,
        },
        ["5"] = {
            sNodeId = "5",
            nTriggerId = 1,
            nNodeType = 15,
        },
        ["6"] = {
            bIsError = true,
            sContext = "WaitTriggerTest",
            sNodeId = "6",
            nNodeType = 1,
        },
        ["7"] = {
            nPosZ = 6.0,
            sNodeId = "7",
            nConfigId = 1,
            nPosY = 0.0,
            nPosX = 0.0,
            nNodeType = 4,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
    },
    sStartNodeId = "1",
    tTransitions = {
        ["2"] = {
            sToNodeId = "3",
            sFromNodeId = "2",
            nPath = 1,
        },
        ["3"] = {
            sToNodeId = "4",
            sFromNodeId = "3",
            nPath = 1,
        },
        ["4"] = {
            sToNodeId = "2",
            sFromNodeId = "1",
            nPath = 1,
        },
        ["5"] = {
            sToNodeId = "7",
            sFromNodeId = "1",
            nPath = 1,
        },
        ["0"] = {
            sToNodeId = "6",
            sFromNodeId = "5",
            nPath = 1,
        },
        ["1"] = {
            sToNodeId = "5",
            sFromNodeId = "1",
            nPath = 1,
        },
    },
}