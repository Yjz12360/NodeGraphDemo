
NodeType = {
    Start = 0,
    Print = 1,
    Delay = 3,
    AddMonster = 4,
    HasMonster = 5,
    WaitMonsterNum = 6,
    WaitMonsterDead = 7,
    WaitEnterTrigger = 15,
    RefreshMonsterGroup = 16,
}

NodeGraphState = {
    Pending = 1,
    Running = 2,
    Finish = 3,
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
}

AIActionState = {
    Running = 1,
    Finish = 2,
}