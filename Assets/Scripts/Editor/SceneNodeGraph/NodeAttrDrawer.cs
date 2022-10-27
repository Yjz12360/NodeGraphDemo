using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class NodeAttrDrawer
    {

        public static void DrawInt(FieldInfo fieldInfo, object data)
        {
            int nOldValue = (int)fieldInfo.GetValue(data);
            int nNewValue = EditorGUILayout.IntField(fieldInfo.Name, nOldValue);
            if (nNewValue != nOldValue)
                fieldInfo.SetValue(data, nNewValue);
        }

        public static void DrawFloat(FieldInfo fieldInfo, object data)
        {
            float nOldValue = (float)fieldInfo.GetValue(data);
            float nNewValue = EditorGUILayout.FloatField(fieldInfo.Name, nOldValue);
            if (nNewValue != nOldValue)
                fieldInfo.SetValue(data, nNewValue);
        }

        public static void DrawString(FieldInfo fieldInfo, object data)
        {
            string sOldValue = (string)fieldInfo.GetValue(data);
            if (sOldValue == null) sOldValue = "";
            string sNewValue = EditorGUILayout.TextField(fieldInfo.Name, sOldValue);
            if (!sNewValue.Equals(sOldValue))
                fieldInfo.SetValue(data, sNewValue);
        }

        public static void DrawBool(FieldInfo fieldInfo, object data)
        {
            bool bOldValue = (bool)fieldInfo.GetValue(data);
            bool bNewValue = EditorGUILayout.Toggle(fieldInfo.Name, bOldValue);
            if (bNewValue != bOldValue)
                fieldInfo.SetValue(data, bNewValue);
        }

        public static void DrawEnum(FieldInfo fieldInfo, object data)
        {
            Enum oldValue = (Enum)fieldInfo.GetValue(data);
            Enum newValue = EditorGUILayout.EnumPopup(fieldInfo.Name, oldValue);
            if (newValue != oldValue)
                fieldInfo.SetValue(data, newValue);
        }

        public static void DrawIntList(FieldInfo fieldInfo, object data)
        {
            List<int> list = (List<int>)fieldInfo.GetValue(data);
            if (list == null)
                list = new List<int>();
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(fieldInfo.Name);
                if(GUILayout.Button("添加"))
                {
                    list.Add(0);
                }
            }
            int removeIndex = -1;
            for(int i = 0; i < list.Count; ++i)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    list[i] = EditorGUILayout.IntField((i + 1).ToString(), list[i]);
                    if (GUILayout.Button("删除"))
                        removeIndex = i;
                }
            }
            if (removeIndex >= 0)
                list.RemoveAt(removeIndex);
        }

        public static void DrawFloatList(FieldInfo fieldInfo, object data)
        {
            List<float> list = (List<float>)fieldInfo.GetValue(data);
            if (list == null)
                list = new List<float>();
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(fieldInfo.Name);
                if (GUILayout.Button("添加"))
                {
                    list.Add(0);
                }
            }
            int removeIndex = -1;
            for (int i = 0; i < list.Count; ++i)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    list[i] = EditorGUILayout.FloatField((i + 1).ToString(), list[i]);
                    if (GUILayout.Button("删除"))
                        removeIndex = i;
                }
            }
            if (removeIndex >= 0)
                list.RemoveAt(removeIndex);
        }

        public static void DrawStringList(FieldInfo fieldInfo, object data)
        {
            List<string> list = (List<string>)fieldInfo.GetValue(data);
            if (list == null)
                list = new List<string>();
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(fieldInfo.Name);
                if (GUILayout.Button("添加"))
                {
                    list.Add("");
                }
            }
            int removeIndex = -1;
            for (int i = 0; i < list.Count; ++i)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    list[i] = EditorGUILayout.TextField((i + 1).ToString(), list[i]);
                    if (GUILayout.Button("删除"))
                        removeIndex = i;
                }
            }
            if (removeIndex >= 0)
                list.RemoveAt(removeIndex);
        }

        public static void DrawAttribute(object data, string exclude)
        {
            if (data == null)
                return;
            Type type = data.GetType();
            if (type != null)
            {
                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (fieldInfo.Name == exclude) continue;
                    Type fieldType = fieldInfo.FieldType;
                    if (fieldType == typeof(int))
                        DrawInt(fieldInfo, data);
                    else if (fieldType == typeof(float))
                        DrawFloat(fieldInfo, data);
                    else if (fieldType == typeof(string))
                        DrawString(fieldInfo, data);
                    else if (fieldType == typeof(bool))
                        DrawBool(fieldInfo, data);
                    else if (fieldType.IsEnum)
                        DrawEnum(fieldInfo, data);
                    else if (fieldType == typeof(List<int>))
                        DrawIntList(fieldInfo, data);
                    else if (fieldType == typeof(List<float>))
                        DrawFloatList(fieldInfo, data);
                    else if (fieldType == typeof(List<string>))
                        DrawStringList(fieldInfo, data);
                }
            }
        }

        public static void DrawNodeData(BaseNode node)
        {
            DrawAttribute(node, "nNodeId");
        }
    }


}