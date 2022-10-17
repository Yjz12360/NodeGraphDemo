using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

[CSharpCallLua]
public static class LuaFuncsCaller
{
    private static LuaEnv luaEnv;
    public static void Initialize(LuaEnv luaEnv)
    {
        LuaFuncsCaller.luaEnv = luaEnv;
    }

    public static void DoCall<T>(string funcName, T param)
    {
        if (luaEnv == null)
        {
            Debug.LogError("CSharp call Lua func error: luaEnv not exists.");
            return;
        }
        Action<T> func = luaEnv.Global.Get<Action<T>>(funcName);
        if (func == null)
        {
            Debug.LogError($"CSharp call Lua func error: func {funcName} not exists.");
            return;
        }
        func(param);
    }

    public static void DoCall<T1, T2>(string funcName, T1 param1, T2 param2)
    {
        if (luaEnv == null)
        {
            Debug.LogError("CSharp call Lua func error: luaEnv not exists.");
            return;
        }
        Action<T1, T2> func = luaEnv.Global.Get<Action<T1, T2>>(funcName);
        if (func == null)
        {
            Debug.LogError($"CSharp call Lua func error: func {funcName} not exists.");
            return;
        }
        func(param1, param2);
    }

}

