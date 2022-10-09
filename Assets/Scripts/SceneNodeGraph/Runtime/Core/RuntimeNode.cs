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
        public bool bFinished;
    }

    public class CltRuntimeNode : BaseRuntimeNode
    {
        public CltNodeGraph nodeGraph;
        public BaseNodeData baseNodeData;
        public virtual void StartNode() { }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void OnFinishNode() { }
        public void FinishNode(int nPath = 1)
        {
            if (bFinished) return;
            bFinished = true;
            OnFinishNode();
            if (nodeGraph != null)
                nodeGraph.FinishNode(sNodeId, nPath);
        }
    }
    public class SvrRuntimeNode : BaseRuntimeNode
    {
        public SvrNodeGraph nodeGraph;
        public BaseNodeData baseNodeData;
        public virtual void StartNode() { }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void OnFinishNode() { }
        public virtual bool SyncFinishNode() { return false; }
        public virtual void OnMonsterDead(int nObjectId) { }
        public virtual void OnMonsterNumChange(int nNum) { }
        public void FinishNode(int nPath = 1)
        {
            if (bFinished) return;
            bFinished = true;
            OnFinishNode();
            if (nodeGraph != null)
                nodeGraph.FinishNode(sNodeId, nPath);
            if (SyncFinishNode())
                NodeGraphMessager.S2CFinishNode(nodeGraph.nNodeGraphId, sNodeId, nPath);
        }
    }
}
