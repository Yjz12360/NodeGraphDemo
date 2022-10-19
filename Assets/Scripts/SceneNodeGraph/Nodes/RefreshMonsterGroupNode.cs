using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class RefreshMonsterGroupNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.RefreshMonsterGroup; }

        public string sRefreshId;
    }

}
