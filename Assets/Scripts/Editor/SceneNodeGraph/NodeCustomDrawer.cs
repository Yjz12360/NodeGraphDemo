using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public class CustomDrawerAttribute : Attribute
    {
        public Type drawerType;
        public CustomDrawerAttribute(Type drawerType)
        {
            if(!drawerType.IsSubclassOf(typeof(BaseCustomDrawer)))
            {
                Debug.LogError("CustomDrawerAttribute Error: not a subtype of BaseCustomDrawer");
                return;
            }
            this.drawerType = drawerType;
        }
    }
    public abstract class BaseCustomDrawer
    {
        public abstract void DrawAttr(FieldInfo fieldInfo, object data);
    }
}