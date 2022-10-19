Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsterGroups = {
        ["2"] = {
            tRefreshMonsters = {"2", "4"},
            sGroupId = "2",
        },
        ["1"] = {
            tRefreshMonsters = {"1", "3"},
            sGroupId = "1",
        },
    },
    tRefreshMonsters = {
        ["2"] = {
            tPos = {z = 0.0, y = 0.0, x = 0.0},
            nMonsterCfgId = 2,
            sRefreshId = "2",
        },
        ["1"] = {
            tPos = {z = 0.0, y = 0.0, x = -0.12},
            nMonsterCfgId = 1,
            sRefreshId = "1",
        },
        ["4"] = {
            tPos = {z = 5.0, y = 0.0, x = 5.0},
            nMonsterCfgId = 4,
            sRefreshId = "4",
        },
        ["3"] = {
            tPos = {z = -0.38, y = 0.0, x = 0.05},
            nMonsterCfgId = 3,
            sRefreshId = "3",
        },
    },
}