Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestFolder = {
    tNodeMap = {
        ["2"] = {
            bIsError = false,
            sNodeId = "2",
            sContext = "11111",
            nNodeType = 1,
        },
        ["1"] = {
            nNodeType = 0,
            sNodeId = "1",
        },
    },
    tTransitions = {
        ["0"] = {
            sFromNodeId = "1",
            nPath = 1,
            sToNodeId = "2",
        },
    },
    sStartNodeId = "1",
}