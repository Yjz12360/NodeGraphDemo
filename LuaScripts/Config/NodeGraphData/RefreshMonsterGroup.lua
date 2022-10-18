Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.RefreshMonsterGroup = {
    sStartNodeId = "1",
    tTransitions = {
        ["0"] = {
            nPath = 1,
            sToNodeId = "2",
            sFromNodeId = "1",
        },
    },
    tNodeMap = {
        ["2"] = {
            nNodeType = 16,
            sNodeId = "2",
            nRefreshId = 1,
        },
        ["1"] = {
            nNodeType = 0,
            sNodeId = "1",
        },
    },
}