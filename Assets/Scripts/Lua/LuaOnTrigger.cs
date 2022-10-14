using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

public class LuaOnTrigger : MonoBehaviour
{
    private InitLua initLua = null;

    void Start()
    {
        GameObject lua = GameObject.Find("Lua");
        initLua = lua.GetComponent<InitLua>();
    }

    private void OnTriggerEnter(Collider other)
    {
        LuaEnv luaEnv = initLua.GetLuaEnv();
        if (luaEnv == null) return;
        Action<GameObject, Collider> triggerEnterFunc = luaEnv.Global.Get<Action<GameObject, Collider>>("OnTriggerEnter");
        if (triggerEnterFunc != null)
            triggerEnterFunc(gameObject, other);
    }
}

