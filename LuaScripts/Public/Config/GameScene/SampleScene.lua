Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        ["4"] = {
            nMonsterCfgId = 2,
            tPos = {x = -2.99, y = 0.0, z = -3.54},
            sRefreshId = "4",
        },
        ["1"] = {
            nMonsterCfgId = 1,
            tPath = {},
            tPos = {x = -3.4, y = 0.0, z = 4.12},
            sRefreshId = "1",
        },
        ["2"] = {
            nMonsterCfgId = 1,
            tPath = {},
            tPos = {x = -1.76, y = 0.0, z = 4.1},
            sRefreshId = "2",
        },
        ["3"] = {
            nMonsterCfgId = 2,
            tPos = {x = -4.45, y = 0.0, z = -1.87},
            sRefreshId = "3",
        },
    },
    tRefreshMonsterGroups = {
        ["1"] = {
            tRefreshMonsters = {"1", "2", "3", "4"},
            sGroupId = "1",
        },
    },
}