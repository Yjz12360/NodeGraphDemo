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

    Action<double> updateFunc;

    // Use this for initialization
    void Start()
    {
        luaenv = LuaHelper.CreateEnv();
        LuaFuncsCaller.Initialize(luaenv);
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

