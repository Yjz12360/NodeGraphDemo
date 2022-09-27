using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneNodeGraph
{
    public class Messager
    {
        public static CltNodeGraph cltNodeGraph = CltNodeGraph.instance;
        public static SvrNodeGraph svrNodeGraph = SvrNodeGraph.instance;

        public static void S2CFinishNode(string sNodeId, int nPath)
        {
            cltNodeGraph.OnS2CFinishNode(sNodeId, nPath);
        }

        //public static void C2SFinishNode(string sNodeId, int nPath)
        //{
        //    svrNodeGraph.OnC2SFinishNode(sNodeId, nPath);
        //}
    }
}
