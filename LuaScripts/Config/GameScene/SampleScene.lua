Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        [1] = {nConfigId = 1, nMonsterCfgId = 1, tPos = {x = 0, y = 0, z = 0}, }, 
        [2] = {nConfigId = 2, nMonsterCfgId = 2, tPos = {x = 0, y = 0, z = 0}, }, 
        [3] = {nConfigId = 3, nMonsterCfgId = 3, tPos = {x = 3, y = 0, z = 3}, }, 
        [4] = {nConfigId = 4, nMonsterCfgId = 4, tPos = {x = 3, y = 0, z = 3}, }, 
    },
    tRefreshMonsterGroups = {
        [1] = {1, 2},
        [2] = {3, 4},
    }
}