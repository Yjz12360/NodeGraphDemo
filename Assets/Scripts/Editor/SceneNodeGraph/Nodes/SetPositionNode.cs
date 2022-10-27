using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class SetPositionNode : BaseNode
    {
        public bool bSetPlayer;
        public int nRefreshId;
        public int nPosId;
        [RequireInt("nPosId", 0)]
        public float nPosX;
        [RequireInt("nPosId", 0)]
        public float nPosY;
        [RequireInt("nPosId", 0)]
        public float nPosZ;
    }
}
