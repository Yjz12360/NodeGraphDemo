using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public enum NodeType
    {
        Start = 0,
        Print = 1,
        Move = 2,
        Delay = 3,
        AddMonster = 4,
        HasMonster = 5,
        WaitMonsterNum = 6,
        WaitMonsterDead = 7,
        Explode = 8,
        //AOE = 9,
        AnimatorCtrl = 10,
        ActiveAI = 11,
        PlayEffect = 12,
        ActiveComponent = 13,
        ActiveSceneObject = 14,
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
}