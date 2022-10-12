using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

public class InitLua : MonoBehaviour
{
    LuaEnv luaenv = null;

    public delegate byte[] CustomLoader(ref string filepath);

    // Use this for initialization
    void Start()
    {
        //LuaEnv.AddL
        luaenv = new LuaEnv();
        luaenv.AddLoader((ref string filename) =>
        {
            string fixFileName = $"{Application.dataPath}/../LuaScripts/{filename}.lua";
            if (!File.Exists(fixFileName))
                return null;
            filename = fixFileName;
            FileStream f = new FileStream(fixFileName, FileMode.Open, FileAccess.Read);
            if (f == null) return null;
            byte[] bytes = new byte[f.Length];
            f.Read(bytes, 0, (int)f.Length);
            return bytes;
        });
        luaenv.DoString("require 'Main'");
    }

    // Update is called once per frame
    void Update()
    {
        if (luaenv != null)
        {
            luaenv.Tick();

            Action<double> updateFunc = luaenv.Global.Get<Action<double>>("Update");
            if (updateFunc != null)
                updateFunc(Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        luaenv.Dispose();
    }
}

