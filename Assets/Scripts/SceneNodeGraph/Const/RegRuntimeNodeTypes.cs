using System;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class RegRuntimeNodeTypes
    {
        //public static Dictionary<NodeType, Type> tCltTypes = new Dictionary<NodeType, Type>();
        //public static Dictionary<NodeType, Type> tSvrTypes = new Dictionary<NodeType, Type>();

        //public static void Initialize()
        //{
        //    Dictionary<string, NodeType> tNodeNamesDic = new Dictionary<string, NodeType>();
        //    foreach(int nCode in Enum.GetValues(typeof(NodeType)))
        //    {
        //        string sName = Enum.GetName(typeof(NodeType), nCode);
        //        tNodeNamesDic[sName] = (NodeType)nCode;
        //    }
        //    Type type = typeof(RegRuntimeNodeTypes);
        //    foreach (Type subType in type.Assembly.GetTypes())
        //    {
        //        string sTypeName = subType.Name;
        //        string sPrefix = sTypeName.Substring(0, 3);
        //        string sName = sTypeName.Substring(3);
        //        if (subType.IsSubclassOf(typeof(CltRuntimeNode)) && sPrefix == "Clt" && tNodeNamesDic.ContainsKey(sName))
        //            tCltTypes[tNodeNamesDic[sName]] = subType;
        //        if (subType.IsSubclassOf(typeof(SvrRuntimeNode)) && sPrefix == "Svr" && tNodeNamesDic.ContainsKey(sName))
        //            tSvrTypes[tNodeNamesDic[sName]] = subType;
        //    }
        //}

        public static Dictionary<NodeType, Type> tCltTypes = new Dictionary<NodeType, Type>
        {
            {NodeType.Start, typeof(CltStartNode) },
            {NodeType.Print, typeof(CltPrintNode) },
            {NodeType.Move, typeof(CltMoveNode) },
            {NodeType.Delay, typeof(CltDelayNode) },
        };

        public static Dictionary<NodeType, Type> tSvrTypes = new Dictionary<NodeType, Type>
        {
            {NodeType.Start, typeof(SvrStartNode) },
            {NodeType.Move, typeof(SvrMoveNode) },
            {NodeType.Delay, typeof(SvrDelayNode) },
            {NodeType.AddMonster, typeof(SvrAddMonsterNode) },
            {NodeType.HasMonster, typeof(SvrHasMonsterNode) },
            {NodeType.WaitMonsterNum, typeof(SvrWaitMonsterNumNode) },
        };
    }
}
