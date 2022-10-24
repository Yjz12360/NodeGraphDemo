Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        ["2"] = {
            nMonsterCfgId = 1,
            sRefreshId = "2",
            tPath = {},
            tPos = {z = 4.1, y = 0.0, x = -1.76},
        },
        ["1"] = {
            nMonsterCfgId = 1,
            sRefreshId = "1",
            tPath = {},
            tPos = {z = 4.12, y = 0.0, x = -3.4},
        },
        ["4"] = {
            nMonsterCfgId = 2,
            sRefreshId = "4",
            tPos = {z = -3.54, y = 0.0, x = -2.99},
        },
        ["3"] = {
            nMonsterCfgId = 2,
            sRefreshId = "3",
            tPos = {z = -1.87, y = 0.0, x = -4.45},
        },
    },
    tPositions = {
        ["2"] = {z = -15.5, y = 10.41, x = 0.0},
        ["1"] = {z = -18.92, y = 12.33, x = -0.14},
    },
    tRefreshMonsterGroups = {
        ["1"] = {
            sGroupId = "1",
            tRefreshMonsters = {"1", "2", "3", "4"},
        },
    },
}