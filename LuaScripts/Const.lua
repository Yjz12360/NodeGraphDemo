
NodeType = {
    Start = 0,
    Print = 1,
    Delay = 3,
    AddMonster = 4,
    HasMonster = 5,
    WaitEnterTrigger = 15,
}

NodeGraphState = {
    Pending = 1,
    Running = 2,
    Finish = 3,
}

NodeState = {
    -- Waiting = 0, -- 等待执行
    Activated = 1, -- 已激活，可以开始执行
    Pending = 2, -- 待处理
    Running = 3, -- 执行中，会每帧更新状态
    Hanging = 4, -- 挂起状态，等待外部事件驱动流程
    -- Finished = 6, --已完成
}

GameObjectType = {
    Player = 1,
    Monster = 2,
}