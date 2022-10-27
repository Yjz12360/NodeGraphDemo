using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    public static class NodeAttrDrawer
    {
        private static void DrawInt(FieldInfo fieldInfo, object data)
        {
            int nOldValue = (int)fieldInfo.GetValue(data);
            int nNewValue = EditorGUILayout.IntField(fieldInfo.Name, nOldValue);
            if (nNewValue != nOldValue)
                fieldInfo.SetValue(data, nNewValue);
        }

        private static void DrawFloat(FieldInfo fieldInfo, object data)
        {
            float nOldValue = (float)fieldInfo.GetValue(data);
            float nNewValue = EditorGUILayout.FloatField(fieldInfo.Name, nOldValue);
            if (nNewValue != nOldValue)
                fieldInfo.SetValue(data, nNewValue);
        }

        private static void DrawString(FieldInfo fieldInfo, object data)
        {
            string sOldValue = (string)fieldInfo.GetValue(data);
            if (sOldValue == null) sOldValue = "";
            string sNewValue = EditorGUILayout.TextField(fieldInfo.Name, sOldValue);
            if (!sNewValue.Equals(sOldValue))
                fieldInfo.SetValue(data, sNewValue);
        }

        private static void DrawBool(FieldInfo fieldInfo, object data)
        {
            bool bOldValue = (bool)fieldInfo.GetValue(data);
            bool bNewValue = EditorGUILayout.Toggle(fieldInfo.Name, bOldValue);
            if (bNewValue != bOldValue)
                fieldInfo.SetValue(data, bNewValue);
        }

        private static void DrawEnum(FieldInfo fieldInfo, object data)
        {
            Enum oldValue = (Enum)fieldInfo.GetValue(data);
            Enum newValue = EditorGUILayout.EnumPopup(fieldInfo.Name, oldValue);
            if (newValue != oldValue)
                fieldInfo.SetValue(data, newValue);
        }

        private static void DrawIntList(FieldInfo fieldInfo, object data)
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

        private static void DrawFloatList(FieldInfo fieldInfo, object data)
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

        private static void DrawStringList(FieldInfo fieldInfo, object data)
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

        private static void DrawAttribute(object data, string exclude)
        {
            if (data == null)
                return;
            Type type = data.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == exclude) continue;
                if (!CheckRequireAttr(fieldInfo, data))
                    continue;

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

        private static bool CheckRequireAttr(FieldInfo fieldInfo, object data)
        {
            Type type = data.GetType();
            RequireBoolAttribute requireBool = fieldInfo.GetCustomAttribute<RequireBoolAttribute>();
            if (requireBool != null)
            {
                string requireAttr = requireBool.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || value.GetType() != typeof(bool))
                    return false;
                if ((bool)value != requireBool.value)
                    return false;
            }
            RequireIntAttribute requireInt = fieldInfo.GetCustomAttribute<RequireIntAttribute>();
            if (requireInt != null)
            {
                string requireAttr = requireInt.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || value.GetType() != typeof(int))
                    return false;
                if ((int)value != 0)
                    return false;
            }
            RequireEnumAttribute requireEnum = fieldInfo.GetCustomAttribute<RequireEnumAttribute>();
            if(requireEnum != null)
            {
                string requireAttr = requireEnum.attrName;
                FieldInfo requireField = type.GetField(requireAttr);
                if (requireField == null)
                    return false;
                object value = requireField.GetValue(data);
                if (value == null || !value.GetType().IsEnum)
                    return false;
                if ((int)value != (int)requireEnum.value)
                    return false;
            }
            return true;
        }
        public static void DrawNodeData(BaseNode node)
        {
            DrawAttribute(node, "nNodeId");
        }
    }


}