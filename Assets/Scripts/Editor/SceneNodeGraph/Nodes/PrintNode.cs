using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public class PrintNode : BaseNode
    {
        //[CustomDrawer(typeof(PrintContextDrawer))]
        public string sContext;
        public bool bIsError;
    }

    //public class PrintContextDrawer : BaseCustomDrawer
    //{
    //    public override void DrawAttr(FieldInfo fieldInfo, object data)
    //    {
    //        string sContext = ((PrintNode)data).sContext;
    //        NodeAttrDrawer.DrawString(fieldInfo, data);
    //        EditorGUILayout.LabelField($"Context: {sContext}");
    //    }
    //}
}

