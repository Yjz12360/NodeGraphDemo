using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class BaseRuntimeNode
    {
        public string sNodeId;
        //public virtual void StartNode() 
        //{

        //}
        //public virtual void UpdateNode(float nDeltaTime) 
        //{

        //}

        //public virtual void FinishNode()
        //{

        //}
    }

    public class CltRuntimeNode : BaseRuntimeNode
    {
        //public static bool bSyncFinishNode = false;

        public CltNodeGraph nodeGraph;
        public BaseNodeData baseNodeData;
        public virtual void StartNode() { throw new NotImplementedException(); }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void OnFinishNode() { }
        public void FinishNode(int nPath = 1)
        {
            OnFinishNode();
            if (nodeGraph != null)
                nodeGraph.FinishNode(sNodeId, nPath);
            //if (bSyncFinishNode)
            //    Messager.C2SFinishNode(sNodeId, nPath);
        }
    }

    public class SvrRuntimeNode : BaseRuntimeNode
    {
        public static bool bSyncFinishNode = false;

        public SvrNodeGraph nodeGraph;
        public BaseNodeData baseNodeData;
        public virtual void StartNode() { throw new NotImplementedException(); }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void OnFinishNode() { }

        public void FinishNode(int nPath = 1)
        {
            OnFinishNode();
            if (nodeGraph != null)
                nodeGraph.FinishNode(sNodeId, nPath);
            if (bSyncFinishNode)
                Messager.S2CFinishNode(sNodeId, nPath);
        }

    }
}
