Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Wall = {
    sStartNodeId = "1",
    tNodeMap = {
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["2"] = {nDelayTime = 4.0, sNodeId = "2", nNodeType = 3},
        ["3"] = {sNodeId = "3", bActive = false, sWallId = "1", nNodeType = 21},
    },
    tTransitions = {
        ["1"] = {sToNodeId = "3", nPath = 1, sFromNodeId = "2"},
        ["0"] = {sToNodeId = "2", nPath = 1, sFromNodeId = "1"},
    },
}