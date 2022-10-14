using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

public class LuaOnTrigger : MonoBehaviour
{
    private LuaEnv luaenv = null;

    void Start()
    {
        GameObject lua = GameObject.Find("Lua");
        luaenv = lua.GetComponent<InitLua>().GetLuaEnv();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}

