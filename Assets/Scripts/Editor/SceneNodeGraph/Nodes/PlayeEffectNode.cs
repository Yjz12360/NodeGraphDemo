using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PlayEffectNode : BaseNode
    {
        public int nEffectId;
        public int nPosId;

        [NodeInput(NodeAttrType.VecPos)]
        [CustomDrawer(typeof(Vector3Drawer))]
        [CustomConverter(typeof(Vector3Converter))]
        public Vector3 tPos;
    }

}
