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
        ComponentSetActive = 13,
        GameObjectSetActive = 14,
        WaitEnterTrigger = 15,
        RefreshMonsterGroup = 16,
    }
}