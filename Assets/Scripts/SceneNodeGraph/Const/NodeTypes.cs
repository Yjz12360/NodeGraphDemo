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
    }
}