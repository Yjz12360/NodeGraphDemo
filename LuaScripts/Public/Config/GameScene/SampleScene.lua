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
            tPos = {z = 0.0, x = 0.0, y = 0.0},
            sRefreshId = "2",
            nMonsterCfgId = 2,
        },
        ["3"] = {
            tPos = {z = 5.0, x = 5.0, y = 0.0},
            sRefreshId = "3",
            nMonsterCfgId = 3,
        },
        ["4"] = {
            tPos = {z = 5.0, x = 5.0, y = 0.0},
            sRefreshId = "4",
            nMonsterCfgId = 4,
        },
        ["1"] = {
            tPos = {z = 0.0, x = 0.0, y = 0.0},
            sRefreshId = "1",
            nMonsterCfgId = 1,
        },
    },
}