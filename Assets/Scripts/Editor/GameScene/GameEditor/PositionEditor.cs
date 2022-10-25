﻿using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(PositionEditorData))]
    public class PositionEditor : Editor
    {
        private Transform editorTrans;
        private Transform rootConfigTrans;
        private Transform configTrans;
        private Transform rootEditorDataTrans;
        private Transform editorDataTrans;

        private void InitTrans()
        {
            MonsterEditorData data = target as MonsterEditorData;
            editorTrans = data.transform;
            rootConfigTrans = editorTrans.parent.Find("Config");
            configTrans = GameEditorHelper.GetOrAddChild(rootConfigTrans, "Position");
            rootEditorDataTrans = editorTrans.parent.Find("EditorData");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "Position");
        }

        public override void OnInspectorGUI()
        {
            if (editorTrans == null)
                InitTrans();

            if (configTrans != null && configTrans.childCount > 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("当前位置配置：");
                    if (GUILayout.Button("排序"))
                        GameEditorHelper.ReorderChildById(configTrans);
                }
                for (int i = 0; i < configTrans.childCount; ++i)
                    DisplayPositionConfig(configTrans.GetChild(i));
                
            }
            else
            {
                EditorGUILayout.HelpBox("当前没有配置位置", MessageType.Info);
            }

            if (GUILayout.Button("添加位置配置"))
            {
                GameObject configObject = GameEditorHelper.AddConfig(configTrans);
                EditorGUIUtility.PingObject(configObject);
            }

            if (editorDataTrans != null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("当前暂存配置：");
                    if (GUILayout.Button("排序"))
                        GameEditorHelper.ReorderChildById(editorDataTrans);
                }
                for (int i = 0; i < editorDataTrans.childCount; ++i)
                    DisplayEditorConfig(editorDataTrans.GetChild(i));
            }
        }

        private void DisplayPositionConfig(Transform child)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
                if (GUILayout.Button("暂存"))
                {
                    GameEditorHelper.TransferTo(child, editorDataTrans);
                }
                if (GUILayout.Button("删除"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
        private void DisplayEditorConfig(Transform child)
        {
            if (child == null) return;
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
                if (GUILayout.Button("启用"))
                {
                    GameEditorHelper.TransferTo(child, configTrans);
                }
            }
        }
    }
}