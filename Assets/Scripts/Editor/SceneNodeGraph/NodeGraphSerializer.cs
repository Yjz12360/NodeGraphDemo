
using System.Text;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace SceneNodeGraph
{
    public class SceneNodeGraphSerializer
    {

        public static string dataPath = Application.dataPath + "/NodeGraphData/";

        public static void SavePath(NodeGraphData nodeGraph, string path)
        {
            string context = JsonConvert.SerializeObject(nodeGraph, Formatting.Indented, NodeGraphConverter.converter);
            File.WriteAllText(path, context);
        }
        public static void Save(NodeGraphData nodeGraph, string fileName)
        {
            string path = dataPath + fileName;
            SavePath(nodeGraph, path);
        }

        public static NodeGraphData LoadPath(string path)
        {
            FileStream f = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[f.Length];
            f.Read(bytes, 0, (int)f.Length);
            string context = Encoding.UTF8.GetString(bytes, 0, (int)f.Length);
            f.Close();
            NodeGraphData nodeGraphData = JsonConvert.DeserializeObject<NodeGraphData>(context, NodeGraphConverter.converter);
            return nodeGraphData;
        }

        public static NodeGraphData Load(string fileName)
        {
            string path = dataPath + fileName;
            return LoadPath(path);
        }

        public static void SaveLua(NodeGraphData nodeGraph, string path)
        {
            string fileName = path.Substring(path.LastIndexOf("/") + 1);
            fileName = fileName.Replace(".json", ".lua");
            if (string.IsNullOrEmpty(fileName) || fileName.Contains(" ") || char.IsDigit(fileName[0]))
            {
                Debug.LogError($"SaveLua: fileName invalid: {fileName}");
                return;
            }    

            string context = JsonConvert.SerializeObject(nodeGraph, Formatting.None, NodeGraphConverter.converter);

            XLua.LuaEnv luaEnv = LuaHelper.CreateEnv();
            luaEnv.DoString("require 'Tools/SerializeTool'");
            object[] resultArray = luaEnv.DoString($"return json2LuaTable('{context}')");
            string fixFileName = fileName.Replace(" ", "").Replace(".lua", "");
            string luaText = "Config = Config or {}\nConfig.NodeGraphData = Config.NodeGraphData or {}\n";
            luaText += $"Config.NodeGraphData.{fixFileName} = {(string)resultArray[0]}";
            luaEnv.Dispose();

            File.WriteAllText(path, luaText);
        }

        public static NodeGraphData LoadLua(string path)
        {
            string fileName = path.Substring(path.LastIndexOf("/") + 1);
            fileName = fileName.Replace(".json", ".lua");

            string luaContext = File.ReadAllText(path);

            XLua.LuaEnv luaEnv = LuaHelper.CreateEnv();
            luaEnv.DoString(luaContext);
            luaEnv.DoString("require 'Tools/SerializeTool'");
            string fixFileName = fileName.Replace(" ", "").Replace(".lua", "");
            luaEnv.DoString($"sJson = luaTable2Json(Config.NodeGraphData.{fixFileName})");

            string json = luaEnv.Global.Get<string>("sJson");
            NodeGraphData nodeGraphData = JsonConvert.DeserializeObject<NodeGraphData>(json, NodeGraphConverter.converter);
            return nodeGraphData;
        }
    }
}
