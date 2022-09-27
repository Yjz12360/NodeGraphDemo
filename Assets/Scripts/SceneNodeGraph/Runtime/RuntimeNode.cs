using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class BaseRuntimeNode
    {
        public virtual void StartNode() 
        {

        }
        public virtual void UpdateNode(float nDeltaTime) 
        {

        }

        public virtual void FinishNode()
        {

        }
    }

    public class CltRuntimeNode : BaseRuntimeNode
    {
        public void RecvRuntimeData(string data)
        {

        }
        public void RecvFinishNode()
        {

        }
    }

    public class SvrRuntimeNode : BaseRuntimeNode
    {
        public void SyncRuntimeData(string data)
        {

        }

        public void SyncFinishNode()
        {

        }
    }
}
