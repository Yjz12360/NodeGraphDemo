Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.Effect = {
    tNodeMap = {
        ["2"] = {sPosId = "3", nNodeType = 12, nEffectId = 1, sNodeId = "2"},
        ["1"] = {sNodeId = "1", nNodeType = 0},
    },
    tTransitions = {
        ["0"] = {nPath = 1, sToNodeId = "2", sFromNodeId = "1"},
    },
    sStartNodeId = "1",
}