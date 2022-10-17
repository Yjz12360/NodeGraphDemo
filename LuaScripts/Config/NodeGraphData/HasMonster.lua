Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.HasMonster = {
    sStartNodeId = "1",
    tTransitions = {
        ["0"] = {
            nPath = 1,
            sToNodeId = "2",
            sFromNodeId = "1",
        },
        ["3"] = {
            nPath = 2,
            sToNodeId = "5",
            sFromNodeId = "3",
        },
        ["2"] = {
            nPath = 1,
            sToNodeId = "4",
            sFromNodeId = "3",
        },
        ["1"] = {
            nPath = 1,
            sToNodeId = "3",
            sFromNodeId = "2",
        },
    },
    tNodeMap = {
        ["5"] = {
            sContext = "No Monster",
            nNodeType = 1,
            bIsError = true,
            sNodeId = "5",
        },
        ["4"] = {
            sContext = "Has Monster",
            nNodeType = 1,
            bIsError = true,
            sNodeId = "4",
        },
        ["3"] = {
            sNodeId = "3",
            nNodeType = 5,
        },
        ["2"] = {
            nPosZ = 5.0,
            sNodeId = "2",
            nPosY = 0.0,
            nNodeType = 4,
            nPosX = 0.0,
            nConfigId = 2,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
    },
}