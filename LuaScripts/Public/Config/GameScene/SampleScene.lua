Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        ["1"] = {
            tPos = {y = 0.0, x = -3.4, z = 4.12},
            sRefreshId = "1",
            nMonsterCfgId = 1,
            tPath = {},
        },
        ["4"] = {
            tPos = {y = 0.0, x = -2.99, z = -3.54},
            sRefreshId = "4",
            nMonsterCfgId = 2,
        },
        ["3"] = {
            tPos = {y = 0.0, x = -4.45, z = -1.87},
            sRefreshId = "3",
            nMonsterCfgId = 2,
        },
        ["2"] = {
            tPos = {y = 0.0, x = -1.76, z = 4.1},
            sRefreshId = "2",
            nMonsterCfgId = 1,
            tPath = {},
        },
    },
    tPositions = {
        ["1"] = {y = 12.33, x = -0.14, z = -18.92},
        ["2"] = {y = 10.41, x = 0.0, z = -15.5},
    },
    tRefreshMonsterGroups = {
        ["1"] = {
            sGroupId = "1",
            tRefreshMonsters = {"1", "2", "3", "4"},
        },
    },
}