using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TriggerEditorData))]
    public class TriggerEditor: Editor
    {
        private Transform editorTrans;
        private Transform rootSceneDataTrans;
        private Transform sceneDataTrans;
        private Transform containerTrans;
        private Transform rootEditorDataTrans;
        private Transform editorDataTrans;

        private void InitTrans()
        {
            TriggerEditorData data = target as TriggerEditorData;
            editorTrans = data.transform;
            rootSceneDataTrans = editorTrans.parent.Find("SceneData");
            sceneDataTrans = rootSceneDataTrans.GetChild(0);
            containerTrans = GameEditorHelper.GetOrAddChild(sceneDataTrans, "Triggers");
            rootEditorDataTrans = editorTrans.parent.Find("EditorData");
            editorDataTrans = GameEditorHelper.GetOrAddChild(rootEditorDataTrans, "Trigger");
        }
        public override void OnInspectorGUI()
        {
            if (editorTrans == null)
                InitTrans();

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("当前触发器：");
                if (GUILayout.Button("排序"))
                    GameEditorHelper.ReorderChildById(containerTrans);
            }
            for (int i = 0; i < containerTrans.childCount; ++i)
            {
                DisplayTrigger(containerTrans.GetChild(i));
            }
            if (GUILayout.Button("添加触发器"))
            {
                int genId = GameEditorHelper.GenChildId(containerTrans);
                GameObject prefab = Resources.Load<GameObject>("GamePrefabs/Trigger/Trigger");
                GameObject newObject = Instantiate(prefab);
                newObject.name = genId.ToString();
                LuaOnTrigger luaOnTrigger = newObject.GetComponent<LuaOnTrigger>();
                if (luaOnTrigger != null)
                    luaOnTrigger.triggerId = genId;
                newObject.transform.SetParent(containerTrans);
                newObject.transform.position = Vector3.zero;
                BoxCollider collider = newObject.GetComponent<BoxCollider>();
                if(collider == null)
                    collider = newObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
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