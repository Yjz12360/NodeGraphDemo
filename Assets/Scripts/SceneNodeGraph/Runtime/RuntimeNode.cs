using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class BaseRuntimeNode
    {
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
        public CltNodeGraph nodeGraph;
        public BaseNodeData nodeData;
        public virtual void StartNode() { throw new NotImplementedException(); }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void FinishNode() { }

        public void DoFinishNode()
        {
            FinishNode();
            //if (nodeGraph != null)
                //nodeGraph.on
        }

        public void RecvRuntimeData(string data)
        {

        }
        public void RecvFinishNode()
        {

        }
    }

    public class SvrRuntimeNode : BaseRuntimeNode
    {
        public SvrNodeGraph nodeGraph;
        public BaseNodeData nodeData;
        public virtual void StartNode() { throw new NotImplementedException(); }
        public virtual void UpdateNode(float nDeltaTime) { }
        public virtual void FinishNode() { }

        public void SyncRuntimeData(string data)
        {

        }

        public void SyncFinishNode()
        {

        }
    }
}
