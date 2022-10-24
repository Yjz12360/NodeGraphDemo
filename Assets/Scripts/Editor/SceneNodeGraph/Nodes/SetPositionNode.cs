using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class SetPositionNode : BaseNode
    {
        public bool bSetPlayer;
        public string sRefreshId;
        public string sPosId;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
    }
}
