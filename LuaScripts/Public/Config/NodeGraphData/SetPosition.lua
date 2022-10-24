Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.SetPosition = {
    tTransitions = {
        ["1"] = {sFromNodeId = "2", sToNodeId = "3", nPath = 1},
        ["0"] = {sFromNodeId = "1", sToNodeId = "2", nPath = 1},
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["3"] = {bSetPlayer = true, sRefreshId = "0", sNodeId = "3", nPosZ = 0.0, nPosX = 0.0, sPosId = "1", nPosY = 0.0, nNodeType = 19},
        ["2"] = {nNodeType = 3, sNodeId = "2", nDelayTime = 2.0},
        ["1"] = {sNodeId = "1", nNodeType = 0},
    },
}