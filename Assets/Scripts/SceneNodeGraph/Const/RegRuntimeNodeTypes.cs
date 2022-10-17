//using System;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SceneNodeGraph
//{
//    public static class RegRuntimeNodeTypes
//    {
//        public static Dictionary<NodeType, Type> tCltTypes = new Dictionary<NodeType, Type>();
//        public static Dictionary<NodeType, Type> tSvrTypes = new Dictionary<NodeType, Type>();

//        static RegRuntimeNodeTypes()
//        {
//            Dictionary<string, NodeType> tNodeNamesDic = new Dictionary<string, NodeType>();
//            Type nodeTypeType = typeof(NodeType);
//            Array tNodeTypes = Enum.GetValues(nodeTypeType);
//            foreach (int nCode in tNodeTypes)
//            {
//                string sName = Enum.GetName(nodeTypeType, nCode);
//                tNodeNamesDic[sName] = (NodeType)nCode;
//            }
//            Type[] allTypes = nodeTypeType.Assembly.GetTypes();
//            foreach (Type subType in allTypes)
//            {
//                string sTypeName = subType.Name;
//                if (sTypeName.Length <= 7) continue;
//                string sPostfix = sTypeName.Substring(sTypeName.Length - 4);
//                if (sPostfix == "Node")
//                {
//                    string sPrefix = sTypeName.Substring(0, 3);
//                    string sName = sTypeName.Substring(3, sTypeName.Length - 7);
//                    if (sPrefix == "Clt" && tNodeNamesDic.ContainsKey(sName) && subType.IsSubclassOf(typeof(CltRuntimeNode)))
//                        tCltTypes[tNodeNamesDic[sName]] = subType;
//                    if (sPrefix == "Svr" && tNodeNamesDic.ContainsKey(sName) && subType.IsSubclassOf(typeof(SvrRuntimeNode)))
//                        tSvrTypes[tNodeNamesDic[sName]] = subType;
//                }
//            }
//        }
//    }
//}
