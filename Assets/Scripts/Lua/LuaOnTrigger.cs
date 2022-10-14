using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.Text;
using System.IO;
using System;

[CSharpCallLua]
public class LuaOnTrigger : MonoBehaviour
{
    private InitLua initLua = null;
    private int triggerId;
    void Start()
    {
        GameObject lua = GameObject.Find("Lua");
        initLua = lua.GetComponent<InitLua>();
        if (!int.TryParse(gameObject.name, out triggerId))
            triggerId = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        LuaEnv luaEnv = initLua.GetLuaEnv();
        if (luaEnv == null) return;
        Action<int , Collider> triggerEnterFunc = luaEnv.Global.Get<Action<int, Collider>>("OnTriggerEnter");
        if (triggerEnterFunc != null)
            triggerEnterFunc(triggerId, other);
    }
}

