Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        ["3"] = {
            tPos = {x = 0.05, y = 0.0, z = -0.38},
            sRefreshId = "3",
            nMonsterCfgId = 3,
        },
        ["4"] = {
            tPos = {x = 5.0, y = 0.0, z = 5.0},
            sRefreshId = "4",
            nMonsterCfgId = 4,
        },
        ["1"] = {
            tPos = {x = -0.12, y = 0.0, z = 0.0},
            sRefreshId = "1",
            nMonsterCfgId = 1,
        },
        ["2"] = {
            tPos = {x = 0.0, y = 0.0, z = 0.0},
            sRefreshId = "2",
            nMonsterCfgId = 2,
        },
    },
    tRefreshMonsterGroups = {
        ["1"] = {
            sGroupId = "1",
            tRefreshMonsters = {"1", "3"},
        },
        ["2"] = {
            sGroupId = "2",
            tRefreshMonsters = {"2", "4"},
        },
    },
}