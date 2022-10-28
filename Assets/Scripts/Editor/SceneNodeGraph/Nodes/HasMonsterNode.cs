using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SceneNodeGraph
{
    public class HasMonsterNode : BaseNode
    {
        public override Type GetPathType() { return typeof(ConditionPath); }
    }

}
