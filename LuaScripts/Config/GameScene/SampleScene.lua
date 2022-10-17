Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tMonsters = {
        ["1"] = {nConfigId = 1, tPos = {x = 0, y = 0, z = 0}, }, 
        ["2"] = {nConfigId = 2, tPos = {x = 0, y = 0, z = 0}, }, 
        ["3"] = {nConfigId = 1, tPos = {x = 3, y = 0, z = 3}, }, 
        ["4"] = {nConfigId = 2, tPos = {x = 3, y = 0, z = 3}, }, 
    },
    tMonsterGroup = {
        ["1"] = {"1", "2"},
        ["2"] = {"3", "4"},
    }
}