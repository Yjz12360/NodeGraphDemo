Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Explode = {
    tNodeMap = {
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["4"] = {nPosZ = 0.0, sNodeId = "4", nNodeType = 4, nPosX = 0.0, nPosY = 0.0, nConfigId = 1, sPosId = "3"},
        ["3"] = {nExplosionId = 1, sPosId = "3", sNodeId = "3", nNodeType = 8},
        ["2"] = {nDelayTime = 1.0, sNodeId = "2", nNodeType = 3},
    },
    sStartNodeId = "1",
    tTransitions = {
        ["1"] = {sToNodeId = "4", nPath = 1, sFromNodeId = "1"},
        ["0"] = {sToNodeId = "3", nPath = 1, sFromNodeId = "2"},
        ["2"] = {sToNodeId = "2", nPath = 1, sFromNodeId = "1"},
    },
}