using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

namespace SceneNodeGraph
{
    public class SceneNodeGraphSerializer
    {

        public static string sDataPath = Application.dataPath + "/NodeGraphData/";

        [MenuItem("Test/保存节点图")]
        public static void TestSave()
        {
            NodeGraphData nodeGraph = new NodeGraphData();

            BaseNodeData startNode = new BaseNodeData();
            startNode.sNodeId = "1";
            nodeGraph.AddNode(startNode);

            PrintNodeData printNode = new PrintNodeData();
            printNode.sNodeId = "2";
            printNode.sContext = "Test Load";
            nodeGraph.AddNode(printNode);

            DelayNodeData delayNode = new DelayNodeData();
            delayNode.sNodeId = "3";
            delayNode.nDelayTime = 1;
            nodeGraph.AddNode(delayNode);

            PrintNodeData printNode2 = new PrintNodeData();
            printNode2.sNodeId = "4";
            printNode2.sContext = "Test Load 2";
            nodeGraph.AddNode(printNode2);

            nodeGraph.AddTransition("1", "2");
            nodeGraph.AddTransition("2", "3");
            nodeGraph.AddTransition("3", "4");

            nodeGraph.SetStartNode(startNode);

            Save(nodeGraph, "test.txt");
        }

        [MenuItem("Test/读取节点图")]
        public static void TestLoad()
        {
            NodeGraphData nodeGraphData = Load("test.txt");
            Save(nodeGraphData, "test2.txt");
        }

        public static void Save(NodeGraphData nodeGraph, string sFileName)
        {
            string sContext = JsonConvert.SerializeObject(nodeGraph, Formatting.Indented, SceneNodeGraphConverter.converter);
            string sPath = sDataPath + sFileName;
            FileStream f = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] bytes = new UTF8Encoding(true).GetBytes(sContext);
            f.Write(bytes, 0, bytes.Length);
            f.Close();
        }

        public static NodeGraphData Load(string sFileName)
        {
            string sPath = sDataPath + sFileName;
            FileStream f = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[f.Length];
            f.Read(bytes, 0, (int)f.Length);
            string sContext = Encoding.UTF8.GetString(bytes, 0, (int)f.Length);
            NodeGraphData nodeGraphData = JsonConvert.DeserializeObject<NodeGraphData>(sContext, SceneNodeGraphConverter.converter);
            return nodeGraphData;
        }

    }
}
