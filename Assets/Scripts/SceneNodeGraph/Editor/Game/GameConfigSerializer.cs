
using System.Text;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEditor;

namespace Game
{
    public class GameConfigSerializer
    {

        public static string sLuaPath = Application.dataPath + "/../LuaScripts/";
        public static string sLuaConfigPath = Application.dataPath + "/../LuaScripts/Public/Config/GameScene/";

        public static void SaveLua(GameObject configObject, string sConfigName)
        {
            if (string.IsNullOrEmpty(sConfigName) || sConfigName.Contains(" ") || char.IsDigit(sConfigName[0]))
            {
                Debug.LogError($"SaveLua: sConfigName invalid: {sConfigName}");
                return;
            }

            string sContext = JsonConvert.SerializeObject(configObject, Formatting.None, GameConfigConverter.converter);

            XLua.LuaEnv luaEnv = new XLua.LuaEnv();
            luaEnv.AddLoader((ref string filename) =>
            {
                string fixFileName = $"{sLuaPath}{filename}.lua";
                string strLuaContent = File.ReadAllText(fixFileName);
                byte[] result = System.Text.Encoding.UTF8.GetBytes(strLuaContent);
                return result;
            });
            luaEnv.DoString("json = require 'Tools/json'");
            luaEnv.DoString("require 'Public/TableUtil'");

            object[] resultArray = luaEnv.DoString($"return table2str(json.decode('{sContext}'))");
            string sLuaText = "Config = Config or {}\nConfig.GameScene = Config.GameScene or {}\n";
            sLuaText += $"Config.GameScene.{sConfigName} = {(string)resultArray[0]}";
            luaEnv.Dispose();

            string sPath = $"{sLuaConfigPath}{sConfigName}.lua";
            File.WriteAllText(sPath, sLuaText);
        }

        public static GameObject LoadLua(string sConfigName)
        {
            string sPath = $"{sLuaConfigPath}{sConfigName}.lua";

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
            luaEnv.DoString("json = require 'Tools/json'");
            luaEnv.DoString($"sJson = json.encode(Config.GameScene.{sConfigName})");
            string sJson = luaEnv.Global.Get<string>("sJson");
            GameObject configObject = JsonConvert.DeserializeObject<GameObject>(sJson, GameConfigConverter.converter);
            return configObject;
        }
    }
}
