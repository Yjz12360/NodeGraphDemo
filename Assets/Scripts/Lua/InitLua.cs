using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

public class InitLua : MonoBehaviour
{
    private LuaEnv luaenv = null;

    public delegate byte[] CustomLoader(ref string filepath);

    Action<double> updateFunc;

    // Use this for initialization
    void Start()
    {
        luaenv = new LuaEnv();
        LuaFuncsCaller.Initialize(luaenv);
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
        luaenv.DoString("require 'Client/Main'");
        updateFunc = luaenv.Global.Get<Action<double>>("Update");
    }

    // Update is called once per frame
    void Update()
    {
        if (luaenv != null)
        {
            luaenv.Tick();

            if (updateFunc != null)
                updateFunc(Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        updateFunc = null;
        luaenv.Dispose();
    }
}

