Config = Config or {}
Config.NodeGraphData = Config.NodeGraphData or {}
Config.NodeGraphData.TestInput = {
    nStartNodeId = 1,
    tNodeInputSource = {
        [5] = {
            tPos = {nInputNodeId = 4, sInputNodeAttr = "tPos"},
        },
    },
    tNodeMap = {
        [1] = {nNodeId = 1, nNodeType = 0},
        [2] = {nNodeId = 2, nNodeType = 4, nPosX = 0.0, nPosY = 0.0, nPosZ = 0.0, nRefreshId = 1},
        [3] = {nDelayTime = 2.0, nNodeId = 3, nNodeType = 3},
        [4] = {nNodeId = 4, nNodeType = 23, nRefreshId = 1},
        [5] = {
            nEffectId = 1,
            nNodeId = 5,
            nNodeType = 12,
            nPosId = 0,
            tPos = {x = 0.0, y = 0.0, z = 0.0},
        },
        [6] = {nDelayTime = 666.0, nNodeId = 6, nNodeType = 3},
    },
    tRequiredOutput = {
        [4] = {tPos = true},
    },
    tTransitions = {
        [1] = {
            [1] = {2},
        },
        [2] = {
            [1] = {3},
        },
        [3] = {
            [1] = {4},
        },
        [4] = {
            [1] = {5, 6},
        },
    },
}