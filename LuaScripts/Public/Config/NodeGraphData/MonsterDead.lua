Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.MonsterDead = {
    tNodeMap = {
        ["2"] = {
            sRefreshId = "1",
            nPosX = 0.0,
            sNodeId = "2",
            nPosY = 0.0,
            nPosZ = 0.0,
            nConfigId = 0,
            nNodeType = 4,
        },
        ["1"] = {
            nNodeType = 0,
            sNodeId = "1",
        },
        ["4"] = {
            nNodeType = 1,
            bIsError = true,
            sContext = "Dead!",
            sNodeId = "4",
        },
        ["3"] = {
            sRefreshId = "1",
            nNodeType = 7,
            sNodeId = "3",
        },
    },
    sStartNodeId = "1",
    tTransitions = {
        ["2"] = {
            sToNodeId = "4",
            nPath = 1,
            sFromNodeId = "3",
        },
        ["1"] = {
            sToNodeId = "3",
            nPath = 1,
            sFromNodeId = "2",
        },
        ["0"] = {
            sToNodeId = "2",
            nPath = 1,
            sFromNodeId = "1",
        },
    },
}