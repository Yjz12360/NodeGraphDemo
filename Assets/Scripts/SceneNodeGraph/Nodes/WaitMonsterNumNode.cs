﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum CompareType
    {
        LessThan = 1,
        MoreThan = 2,
        Equal = 3,
    }
    public class WaitMonsterNumNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.WaitMonsterNum; }

        public CompareType nCompareType = CompareType.LessThan;
        public int nNum = 0;
        public string sMonsterGroupId = "";
    }
}
