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
    [HideInInspector]
    public int triggerId;
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        LuaFuncsCaller.DoCall("OnTriggerEnter", triggerId, other);
    }
}

