Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Path = {
    sStartNodeId = "1",
    tTransitions = {
        ["0"] = {sFromNodeId = "1", sToNodeId = "2", nPath = 1},
    },
    tNodeMap = {
        ["1"] = {nNodeType = 0, sNodeId = "1"},
        ["2"] = {nNodeType = 16, sNodeId = "2", sRefreshId = "1"},
    },
}