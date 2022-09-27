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
            nodeGraph.AddTransition("1", "2");

            nodeGraph.SetStartNode(startNode);

            Save(nodeGraph, "test.txt");
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

    }
}
