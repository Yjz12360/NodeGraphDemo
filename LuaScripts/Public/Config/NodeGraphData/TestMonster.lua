Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestMonster = {
    tNodeMap = {
        ["3"] = {sNodeId = "3", nDelayTime = 2.0, nNodeType = 3},
        ["2"] = {sNodeId = "2", nNodeType = 16, sRefreshId = "1"},
        ["1"] = {nNodeType = 0, sNodeId = "1"},
        ["4"] = {sNodeId = "4", nNodeType = 16, sRefreshId = "2"},
    },
    tTransitions = {
        ["2"] = {sFromNodeId = "3", sToNodeId = "4", nPath = 1},
        ["1"] = {sFromNodeId = "2", sToNodeId = "3", nPath = 1},
        ["0"] = {sFromNodeId = "1", sToNodeId = "2", nPath = 1},
    },
    sStartNodeId = "1",
}