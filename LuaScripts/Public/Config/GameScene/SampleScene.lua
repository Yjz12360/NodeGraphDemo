Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        [1] = {
            tPath = {},
            nRefreshId = 1,
            nMonsterCfgId = 1,
            tPos = {x = -3.4, y = 0.0, z = 4.12},
        },
        [2] = {
            tPath = {},
            nRefreshId = 2,
            nMonsterCfgId = 1,
            tPos = {x = -1.76, y = 0.0, z = 4.1},
        },
        [3] = {
            nMonsterCfgId = 2,
            nRefreshId = 3,
            tPos = {x = -4.45, y = 0.0, z = -1.87},
        },
        [4] = {
            nMonsterCfgId = 2,
            nRefreshId = 4,
            tPos = {x = -2.99, y = 0.0, z = -3.54},
        },
    },
    tRefreshMonsterGroups = {
        [1] = {
            tRefreshMonsters = {1, 2, 3, 4},
            nGroupId = 1,
        },
    },
    tPositions = {
        [1] = {x = -0.14, y = 12.33, z = -18.92},
        [2] = {x = 0.0, y = 10.41, z = -15.5},
        [3] = {x = 0.0, y = 0.0, z = 0.0},
    },
}