Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.GroupMonsterNum = {
    tTransitions = {
        ["3"] = {sToNodeId = "5", nPath = 1, sFromNodeId = "4"},
        ["0"] = {sToNodeId = "2", nPath = 1, sFromNodeId = "1"},
        ["1"] = {sToNodeId = "3", nPath = 1, sFromNodeId = "1"},
        ["2"] = {sToNodeId = "4", nPath = 1, sFromNodeId = "2"},
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["3"] = {nNodeType = 16, sRefreshId = "2", sNodeId = "3"},
        ["4"] = {nCompareType = 1, sNodeId = "4", nNodeType = 6, nNum = 1, sMonsterGroupId = "1"},
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["2"] = {nNodeType = 16, sRefreshId = "1", sNodeId = "2"},
        ["5"] = {sContext = "GroupNum Test", nNodeType = 1, bIsError = true, sNodeId = "5"},
    },
}