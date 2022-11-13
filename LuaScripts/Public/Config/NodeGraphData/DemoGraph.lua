Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.DemoGraph = {
    nStartNodeId = 1,
    tNodeInputSource = {},
    tNodeMap = {
        [1] = {nNodeId = 1, nNodeType = 0},
        [2] = {bInfinite = false, nGroupId = 1, nNodeId = 2, nNodeType = 16, nRefreshCount = 1, nRefreshInv = 2.0},
        [3] = {nCompareType = 3, nNodeId = 3, nNodeType = 6, nNum = 2, nGroupId = 1},
        [4] = {bIsError = false, nNodeId = 4, nNodeType = 1, sContext = "Monster Group 2"},
        [5] = {nCompareType = 3, nNodeId = 5, nNodeType = 6, nNum = 1, nGroupId = 1},
        [6] = {bIsError = false, nNodeId = 6, nNodeType = 1, sContext = "Monster Group 1"},
        [7] = {nCompareType = 3, nNodeId = 7, nNodeType = 6, nNum = 0, nGroupId = 1},
        [8] = {bIsError = false, nNodeId = 8, nNodeType = 1, sContext = "Monster Group 0"},
        [9] = {nNodeId = 9, nNodeType = 4, nPosX = 0.0, nPosY = 0.0, nPosZ = 0.0, nRefreshId = 1},
        [10] = {nNodeId = 10, nNodeType = 7, nRefreshId = 1},
        [11] = {bIsError = false, nNodeId = 11, nNodeType = 1, sContext = "Monster Dead"},
        [12] = {nDelayTime = 1.0, nNodeId = 12, nNodeType = 3},
        [13] = {bIsError = false, nNodeId = 13, nNodeType = 1, sContext = "Delay 1"},
        [14] = {nDelayTime = 2.0, nNodeId = 14, nNodeType = 3},
        [15] = {bIsError = false, nNodeId = 15, nNodeType = 1, sContext = "Delay 2"},
        [16] = {nDelayTime = 3.0, nNodeId = 16, nNodeType = 3},
        [17] = {bIsError = false, nNodeId = 17, nNodeType = 1, sContext = "Delay 3"},
    },
    tRequiredOutput = {},
    tTransitions = {
        [1] = {
            [1] = {2, 9, 12, 14, 16},
        },
        [2] = {
            [1] = {3, 5, 7},
        },
        [3] = {
            [1] = {4},
        },
        [5] = {
            [1] = {6},
        },
        [7] = {
            [1] = {8},
        },
        [9] = {
            [1] = {10},
        },
        [10] = {
            [1] = {11},
        },
        [12] = {
            [1] = {13},
        },
        [14] = {
            [1] = {15},
        },
        [16] = {
            [1] = {17},
        },
    },
}