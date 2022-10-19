Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.MonsterDead = {
    tTransitions = {
        ["1"] = {sFromNodeId = "2", sToNodeId = "3", nPath = 1},
        ["0"] = {sFromNodeId = "1", sToNodeId = "2", nPath = 1},
        ["2"] = {sFromNodeId = "3", sToNodeId = "4", nPath = 1},
    },
    sStartNodeId = "1",
    tNodeMap = {
        ["1"] = {sNodeId = "1", nNodeType = 0},
        ["4"] = {sNodeId = "4", bIsError = true, sContext = "Dead!", nNodeType = 1},
        ["3"] = {sRefreshId = "1", sNodeId = "3", nNodeType = 7},
        ["2"] = {sNodeId = "2", nPosZ = 0.0, nPosX = 0.0, sRefreshId = "1", nPosY = 0.0, nConfigId = 0, nNodeType = 4},
    },
}