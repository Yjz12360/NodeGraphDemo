Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestFolder = {
    sStartNodeId = "1",
    tTransitions = {
        ["0"] = {
            sToNodeId = "2",
            sFromNodeId = "1",
            nPath = 1,
        },
    },
    tNodeMap = {
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["2"] = {
            bIsError = false,
            sNodeId = "2",
            sContext = "222222",
            nNodeType = 1,
        },
    },
}