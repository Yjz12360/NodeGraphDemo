Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestLua = {
    tTransitions = {
        ["0"] = {
            sToNodeId = "2",
            nPath = 1,
            sFromNodeId = "1",
        },
        ["1"] = {
            sToNodeId = "3",
            nPath = 1,
            sFromNodeId = "2",
        },
        ["2"] = {
            sToNodeId = "4",
            nPath = 1,
            sFromNodeId = "3",
        },
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["4"] = {
            sContext = "Print 2",
            nNodeType = 1,
            sNodeId = "4",
            bIsError = true,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["2"] = {
            sContext = "Print 1",
            nNodeType = 1,
            sNodeId = "2",
            bIsError = true,
        },
        ["3"] = {
            nNodeType = 3,
            sNodeId = "3",
            nDelayTime = 1.0,
        },
    },
}