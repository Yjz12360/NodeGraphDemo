using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TransparentWallEditorData))]
    public class TransparentWallEditor : Editor
    {
        private Transform editorTrans;
        private Transform rootSceneDataTrans;
        private Transform sceneDataTrans;
        private Transform containerTrans;
        private Transform rootEditorDataTrans;
        private Transform editorDataTrans;

        private void InitTrans()
        {
            TransparentWallEditorData data = target as TransparentWallEditorData;
            editorTrans = data.transform;
            rootSceneDataTrans = editorTrans.parent.Find("SceneData");
            sceneDataTrans = rootSceneDataTrans.GetChild(0);
            containerTrans = GameEditorHelper.GetOrAddChild(sceneDataTrans, "TransparentWall");
            rootEditorDataTrans = editorTrans.parent.Find("EditorData");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "TransparentWall");
        }

        public override void OnInspectorGUI()
        {
            if (editorTrans == null)
                InitTrans();

            if (containerTrans != null)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("当前空气墙配置：");
                    if (GUILayout.Button("排序"))
                        GameEditorHelper.ReorderChildById(containerTrans);
                }
                for (int i = 0; i < containerTrans.childCount; ++i)
                    DisplayTrigger(containerTrans.GetChild(i));
            }
            if (GUILayout.Button("添加空气墙"))
            {
                GameObject newObject = GameEditorHelper.AddConfig(containerTrans);
                BoxCollider collider = newObject.GetComponent<BoxCollider>();
                if (collider == null)
                    collider = newObject.AddComponent<BoxCollider>();
                collider.isTrigger = false;
                collider.center = Vector3.zero;
                EditorGUIUtility.PingObject(newObject);
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

        private void DisplayTrigger(Transform child)
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
                    GameEditorHelper.TransferTo(child, containerTrans);
                }
            }
        }
    }
}