Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.MonsterNum = {
    sStartNodeId = "1",
    tTransitions = {
        ["2"] = {
            sToNodeId = "4",
            sFromNodeId = "3",
            nPath = 1,
        },
        ["1"] = {
            sToNodeId = "3",
            sFromNodeId = "2",
            nPath = 1,
        },
        ["0"] = {
            sToNodeId = "2",
            sFromNodeId = "1",
            nPath = 1,
        },
    },
    tNodeMap = {
        ["2"] = {
            nPosX = 0.0,
            nPosZ = 4.0,
            sNodeId = "2",
            nNodeType = 4,
            nPosY = 0.0,
            nConfigId = 2,
        },
        ["1"] = {
            sNodeId = "1",
            nNodeType = 0,
        },
        ["4"] = {
            sNodeId = "4",
            nNodeType = 1,
            bIsError = true,
            sContext = "Test 11111",
        },
        ["3"] = {
            nNum = 1,
            nNodeType = 6,
            nCompareType = 1,
            sNodeId = "3",
        },
    },
}