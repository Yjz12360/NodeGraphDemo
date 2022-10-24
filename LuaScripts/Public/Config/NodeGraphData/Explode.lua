Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Explode = {
    tTransitions = {
        ["1"] = {sToNodeId = "2", nPath = 1, sFromNodeId = "1"},
        ["0"] = {sToNodeId = "3", nPath = 1, sFromNodeId = "2"},
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["3"] = {sPosId = "3", nExplosionId = 1, sNodeId = "3", nNodeType = 8},
        ["2"] = {sNodeId = "2", nDelayTime = 1.0, nNodeType = 3},
    },
}