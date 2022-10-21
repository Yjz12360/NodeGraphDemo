Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        ["1"] = {
            tPath = {
                ["2"] = {x = -0.58, z = 2.64, y = 0.0},
                ["1"] = {x = 1.73, z = 1.99, y = 0.0},
            },
            tPos = {x = -0.12, z = 0.0, y = 0.0},
            sRefreshId = "1",
            nMonsterCfgId = 1,
        },
    },
    tRefreshMonsterGroups = {
        ["1"] = {
            tRefreshMonsters = {"1"},
            sGroupId = "1",
        },
    },
}