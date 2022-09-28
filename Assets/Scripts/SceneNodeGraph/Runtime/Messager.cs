using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class Messager
    {
        public static void S2CFinishNode(int nGraphNodeId, string sNodeId, int nPath)
        {
            CltNodeGraph cltNodeGraph = CltNodeGraphManager.GetNodeGraph(nGraphNodeId);
            if (cltNodeGraph == null) return;
            cltNodeGraph.OnS2CFinishNode(sNodeId, nPath);
        }
    }
}
