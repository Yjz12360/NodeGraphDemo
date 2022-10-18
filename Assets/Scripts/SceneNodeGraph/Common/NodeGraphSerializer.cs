
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
        public static string sLuaPath = Application.dataPath + "/../LuaScripts/";
        public static string sLuaConfigPath = Application.dataPath + "/../LuaScripts/Public/Config/NodeGraphData/";

        public static void SavePath(NodeGraphData nodeGraph, string sPath)
        {
            string sContext = JsonConvert.SerializeObject(nodeGraph, Formatting.Indented, NodeGraphConverter.converter);
            File.WriteAllText(sPath, sContext);
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

        public static void SaveLua(NodeGraphData nodeGraph, string sPath)
        {
            string sFileName = sPath.Substring(sPath.LastIndexOf("/") + 1);
            sFileName = sFileName.Replace(".json", ".lua");
            if (string.IsNullOrEmpty(sFileName) || sFileName.Contains(" ") || char.IsDigit(sFileName[0]))
            {
                Debug.LogError($"SaveLua: fileName invalid: {sFileName}");
                return;
            }    

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
            luaEnv.Dispose();

            File.WriteAllText(sPath, sLuaText);
        }

        public static NodeGraphData LoadLua(string sPath)
        {
            string sFileName = sPath.Substring(sPath.LastIndexOf("/") + 1);
            sFileName = sFileName.Replace(".json", ".lua");

            string sLuaContext = File.ReadAllText(sPath);

            XLua.LuaEnv luaEnv = new XLua.LuaEnv();
            luaEnv.AddLoader((ref string filename) =>
            {
                string fixFileName = $"{sLuaPath}{filename}.lua";
                string strLuaContent = File.ReadAllText(fixFileName);
                byte[] result = System.Text.Encoding.UTF8.GetBytes(strLuaContent);
                return result;
            });

            luaEnv.DoString(sLuaContext);
            luaEnv.DoString("json = require 'json'");
            string sFixFileName = sFileName.Replace(" ", "").Replace(".lua", "");
            luaEnv.DoString($"sJson = json.encode(Config.NodeGraphData.{sFixFileName})");
            string sJson = luaEnv.Global.Get<string>("sJson");
            NodeGraphData nodeGraphData = JsonConvert.DeserializeObject<NodeGraphData>(sJson, NodeGraphConverter.converter);
            return nodeGraphData;
        }
    }
}
