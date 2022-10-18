Config = Config or {}
Config.GameScene = Config.GameScene or {}
Config.GameScene.SampleScene = {
    tRefreshMonsters = {
        [1] = {nRefreshId = 1, nMonsterCfgId = 1, tPos = {x = 0, y = 0, z = 0}, }, 
        [2] = {nRefreshId = 2, nMonsterCfgId = 2, tPos = {x = 0, y = 0, z = 0}, }, 
        [3] = {nRefreshId = 3, nMonsterCfgId = 3, tPos = {x = 3, y = 0, z = 3}, }, 
        [4] = {nRefreshId = 4, nMonsterCfgId = 4, tPos = {x = 3, y = 0, z = 3}, }, 
    },
    tRefreshMonsterGroups = {
        [1] = {nRefreshId = 1, tRefreshMonsters = {1, 3}, },
        [2] = {nRefreshId = 1, tRefreshMonsters = {2, 4}, },
    }
}