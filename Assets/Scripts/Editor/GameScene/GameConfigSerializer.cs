
using System.Text;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace Game
{
    public class GameConfigSerializer
    {

        public static string luaPath = Application.dataPath + "/../LuaScripts/";
        public static string luaConfigPath = Application.dataPath + "/../LuaScripts/Public/Config/GameScene/";

        public static void SaveLua(GameObject configObject, string configName)
        {
            if (string.IsNullOrEmpty(configName) || configName.Contains(" ") || char.IsDigit(configName[0]))
            {
                Debug.LogError($"SaveLua: sConfigName invalid: {configName}");
                return;
            }

            string context = JsonConvert.SerializeObject(configObject, Formatting.None, GameConfigConverter.converter);

            XLua.LuaEnv luaEnv = new XLua.LuaEnv();
            luaEnv.AddLoader((ref string filename) =>
            {
                string fixFileName = $"{luaPath}{filename}.lua";
                string luaContent = File.ReadAllText(fixFileName);
                byte[] result = System.Text.Encoding.UTF8.GetBytes(luaContent);
                return result;
            });

            luaEnv.DoString("require 'Tools/SerializeTool'");
            object[] resultArray = luaEnv.DoString($"return json2LuaTable('{context}')");
            string luaText = "Config = Config or {}\nConfig.GameScene = Config.GameScene or {}\n";
            luaText += $"Config.GameScene.{configName} = {(string)resultArray[0]}";
            luaEnv.Dispose();

            string path = $"{luaConfigPath}{configName}.lua";
            File.WriteAllText(path, luaText);
        }

        public static GameObject LoadLua(string configName)
        {
            string path = $"{luaConfigPath}{configName}.lua";

            if (!File.Exists(path))
                return null;

            string luaContext = File.ReadAllText(path);

            XLua.LuaEnv luaEnv = new XLua.LuaEnv();
            luaEnv.AddLoader((ref string filename) =>
            {
                string fixFileName = $"{luaPath}{filename}.lua";
                string luaContent = File.ReadAllText(fixFileName);
                byte[] result = System.Text.Encoding.UTF8.GetBytes(luaContent);
                return result;
            });

            luaEnv.DoString(luaContext);
            luaEnv.DoString("require 'Tools/SerializeTool'");
            luaEnv.DoString($"sJson = luaTable2Json(Config.GameScene.{configName})");
            string json = luaEnv.Global.Get<string>("sJson");
            GameObject configObject = JsonConvert.DeserializeObject<GameObject>(json, GameConfigConverter.converter);
            return configObject;
        }
    }
}
