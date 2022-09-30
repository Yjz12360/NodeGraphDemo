using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SceneNodeGraph
{
    public class Messager
    {
        private static NodeGraphServer SvrComp = null;
        private static NodeGraphServer ServerComp
        {
            get
            {
                if (SvrComp == null)
                    SvrComp = GameObject.Find("NetWork/Server").GetComponent<NodeGraphServer>();
                return SvrComp;
            }
        }

        private static NodeGraphClient CltComp = null;
        private static NodeGraphClient ClientComp
        {
            get
            {
                if(CltComp == null)
                    CltComp = GameObject.Find("NetWork/Client").GetComponent<NodeGraphClient>();
                return CltComp;
            }
        }

        public static void S2CFinishNode(int nGraphNodeId, string sNodeId, int nPath)
        {
            ClientComp.FinishNode(nGraphNodeId, sNodeId, nPath);
        }

        public static void S2CFinishNodeGraph(int nGraphNodeId)
        {
            ClientComp.FinishNodeGraph(nGraphNodeId);
        }

        public static void S2CActivateNodeGraph(int nGraphNodeId, string sConfigFile)
        {
            ClientComp.ActivateNodeGraph(nGraphNodeId, sConfigFile);
        }

        public static void C2STriggerNodeGraph(string sConfigFile)
        {
            ServerComp.OnTriggerNodeGraph(sConfigFile);
        }
    }
}
