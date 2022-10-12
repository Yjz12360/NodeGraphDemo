
using System.Text;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace SceneNodeGraph
{
    public class SceneNodeGraphSerializer
    {

        public static string sDataPath = Application.dataPath + "/NodeGraphData/";
        public static string sLuaPath = Application.dataPath + "/../../LuaScripts/";
        public static string sLuaConfigPath = Application.dataPath + "/../../LuaScripts/Config/NodeGraphData/";

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

            Save(nodeGraph, "test.json");
        }

        [MenuItem("Test/读取节点图")]
        public static void TestLoad()
        {
            NodeGraphData nodeGraphData = Load("test.json");
            Save(nodeGraphData, "test2.json");
        }

        public static void SavePath(NodeGraphData nodeGraph, string sPath)
        {
            string sContext = JsonConvert.SerializeObject(nodeGraph, Formatting.Indented, NodeGraphConverter.converter);
            File.WriteAllText(sPath, sContext);
            //FileStream f = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write);
            //byte[] bytes = new UTF8Encoding(true).GetBytes(sContext);
            //f.Write(bytes, 0, bytes.Length);
            //f.Close();
        }
        public static void Save(NodeGraphData nodeGraph, string sFileName)
        {
            string sPath = sDataPath + sFileName;
            SavePath(nodeGraph, sPath);
        }

        public static NodeGraphData LoadPath(string sPath)
        {
            FileStream f = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[f.Length];
            f.Read(bytes, 0, (int)f.Length);
            string sContext = Encoding.UTF8.GetString(bytes, 0, (int)f.Length);
            f.Close();
            NodeGraphData nodeGraphData = JsonConvert.DeserializeObject<NodeGraphData>(sContext, NodeGraphConverter.converter);
            return nodeGraphData;
        }

        public static NodeGraphData Load(string sFileName)
        {
            string sPath = sDataPath + sFileName;
            return LoadPath(sPath);
        }

        public static void SaveLua(NodeGraphData nodeGraph, string sFileName)
        {
            string sPath = sLuaConfigPath + sFileName;
            string sContext = JsonConvert.SerializeObject(nodeGraph, Formatting.None, NodeGraphConverter.converter);

            XLua.LuaEnv luaEnv = new XLua.LuaEnv();
            luaEnv.AddLoader((ref string filename) =>
            {
                string fixFileName = $"{sLuaPath}{filename}.lua";
                string strLuaContent = File.ReadAllText(fixFileName);
                byte[] result = System.Text.Encoding.UTF8.GetBytes(strLuaContent);
                return result;
            });
            luaEnv.DoString("json = require 'json'");
            luaEnv.DoString("require 'TableUtil'");

            object[] resultArray = luaEnv.DoString($"return table2str(json.decode('{sContext}'))");
            string sFixFileName = sFileName.Replace(" ", "").Replace(".lua", "");
            string sLuaText = "Config = Config or {}\nConfig.NodeGraphData = Config.NodeGraphData or {}\n";
            sLuaText += $"Config.NodeGraphData.{sFixFileName} = {(string)resultArray[0]}";
            //string sLuaText = string.Format("tConfig = tConfig or {}\n tConfig.{0} = {1}", sFileName, (string)resultArray[0]);

            File.WriteAllText(sPath, sLuaText);
            luaEnv.Dispose();
        }
    }
}
