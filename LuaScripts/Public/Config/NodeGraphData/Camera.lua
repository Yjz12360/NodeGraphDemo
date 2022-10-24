Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Camera = {
    tTransitions = {
        ["2"] = {nPath = 1, sFromNodeId = "3", sToNodeId = "4"},
        ["1"] = {nPath = 1, sFromNodeId = "2", sToNodeId = "3"},
        ["0"] = {nPath = 1, sFromNodeId = "1", sToNodeId = "2"},
    },
    tNodeMap = {
        ["2"] = {nDelayTime = 1.0, sNodeId = "2", nNodeType = 3},
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["4"] = {sContext = "Trace", sNodeId = "4", bIsError = true, nNodeType = 1},
        ["3"] = {sEndPosId = "2", sStartPosId = "1", nMoveTime = 3.0, sNodeId = "3", nNodeType = 20},
    },
    sStartNodeId = "1",
}