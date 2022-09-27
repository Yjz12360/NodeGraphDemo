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

        public delegate void MessageHandle();
        public static void DoNextFrame(MessageHandle handle)
        {
            Task t = Task.Run(async delegate
            {
                await Task.Delay(1);
                handle();
            });
        }

        public static void S2CFinishNode(string sNodeId)
        {
            DoNextFrame(() => { cltNodeGraph.RecvFinishNode(sNodeId); });
        }
    }
}
