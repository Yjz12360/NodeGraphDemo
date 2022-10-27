using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class RefreshMonsterGroupNode : BaseNode
    {
        public int nGroupId;
        public bool bInfinite = false;
        [RequireBool("bInfinite", false)]
        public int nRefreshCount = 1;
        public float nRefreshInv = 2;
    }

}
