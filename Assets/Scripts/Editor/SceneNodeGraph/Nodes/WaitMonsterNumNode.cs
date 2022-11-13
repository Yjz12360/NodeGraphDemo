using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public enum CompareType
    {
        LessThan = 1,
        MoreThan = 2,
        Equal = 3,
    }
    public class WaitMonsterNumNode : BaseNode
    {
        public CompareType nCompareType = CompareType.LessThan;
        public int nNum = 0;
        public int nGroupId = 0;
    }
}
