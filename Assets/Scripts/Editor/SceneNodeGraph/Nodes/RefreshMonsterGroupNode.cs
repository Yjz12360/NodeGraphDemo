using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class RefreshMonsterGroupNode : BaseNode
    {
        public int nGroupId;
        public int nRefreshCount = 1;
        public bool bInfinite = false;
        public float nRefreshInv = 2;
    }

}
