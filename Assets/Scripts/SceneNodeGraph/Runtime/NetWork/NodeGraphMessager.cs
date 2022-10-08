﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class NodeGraphMessager
    {
        private static SvrNodeGraphManager SvrComp = null;
        private static SvrNodeGraphManager ServerComp
        {
            get
            {
                if (SvrComp == null)
                    SvrComp = GameObject.Find("NetWork/Server").GetComponent<SvrNodeGraphManager>();
                return SvrComp;
            }
        }

        private static CltNodeGraphManager CltComp = null;
        private static CltNodeGraphManager ClientComp
        {
            get
            {
                if(CltComp == null)
                    CltComp = GameObject.Find("NetWork/Client").GetComponent<CltNodeGraphManager>();
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
    }
}