
NodeType = {
    Start = 0,
    Print = 1,
    Delay = 3,
    AddMonster = 4,
    HasMonster = 5,
    WaitMonsterNum = 6,
    WaitMonsterDead = 7,
    Explode = 8,
    -- AOE = 9,
    AnimatorCtrl = 10,
    ActiveAI = 11,
    PlayEffect = 12,
    -- ActiveComponent = 13,
    -- ActiveSceneObject = 14,
    WaitEnterTrigger = 15,
    RefreshMonsterGroup = 16,
    Random = 17,
    WaitAllNodeFinish = 18,
    SetPosition = 19,
    CameraTrace = 20,
    ActiveTransparentWall = 21,
    MonsterHpRate = 22,
    GetMonsterPos = 23,
}

GameObjectType = {
    Player = 1,
    Monster = 2,
}

EventType = {
    EnterTrigger = 1,
    AddMonster = 2,
    MonsterDead = 3,
    BeforeMonsterDead = 4,
    FinishNode = 5,
}

AIActionType = {
    Idle = 1,
    MoveTo = 2,
    Chase = 3,
}

CameraMode = {
    None = 0,
    Follow = 1,
    Trace = 2,
}