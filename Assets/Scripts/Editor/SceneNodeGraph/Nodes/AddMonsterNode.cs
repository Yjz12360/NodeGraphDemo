using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AddMonsterNode : BaseNode
    {
        public int nRefreshId;
        [RequireInt("nRefreshId", 0)]
        public int nConfigId;
        [RequireInt("nRefreshId", 0)]
        public int nPosId;
        [RequireInt("nPosId", 0)]
        public float nPosX;
        [RequireInt("nPosId", 0)]
        public float nPosY;
        [RequireInt("nPosId", 0)]
        public float nPosZ;
    }

}
