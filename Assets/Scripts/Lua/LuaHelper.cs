using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

[CSharpCallLua]
public static class LuaHelper
{
    public static LuaEnv CreateEnv()
    {
        LuaEnv luaenv = new LuaEnv();
        luaenv.AddLoader((ref string filename) =>
        {
            string luaFileName = $"{Application.dataPath}/../LuaScripts/{filename}.lua";
            if (!File.Exists(luaFileName))
                return null;
            filename = luaFileName;
            FileStream f = new FileStream(luaFileName, FileMode.Open, FileAccess.Read);
            if (f == null) return null;
            byte[] bytes = new byte[f.Length];
            f.Read(bytes, 0, (int)f.Length);
            return bytes;
        });
        return luaenv;
    }

}

