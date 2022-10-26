using UnityEditor;
using UnityEngine;

namespace Game
{
    public class BaseGameEditor : Editor
    {
        protected Transform editorTrans;
        protected Transform rootConfigTrans;
        protected Transform rootSceneDataTrans;
        protected Transform rootEditorDataTrans;

        protected Transform containerTrans;
        protected Transform editorDataTrans;

        protected virtual void InitTrans()
        {
            editorTrans = ((MonoBehaviour)target).transform;
            rootConfigTrans = editorTrans.parent.Find("Config");
            rootSceneDataTrans = editorTrans.parent.Find("SceneData").GetChild(0);
            rootEditorDataTrans = editorTrans.parent.Find("EditorData");
        }

        protected virtual GameObject AddConfig()
        {
            if (containerTrans == null)
                return null;
            GameObject configObject = GameEditorHelper.AddConfig(containerTrans);
            EditorGUIUtility.PingObject(configObject);
            return configObject;
        }

        public override void OnInspectorGUI()
        {
            if (editorTrans == null)
                InitTrans();

            if (containerTrans != null && containerTrans.childCount > 0)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("当前配置：");
                    if (GUILayout.Button("排序"))
                        GameEditorHelper.ReorderChildById(containerTrans);
                }
                for (int i = 0; i < containerTrans.childCount; ++i)
                    DisplayRuntimeConfig(containerTrans.GetChild(i));
                
            }
            else
            {
                EditorGUILayout.HelpBox("当前没有配置", MessageType.Info);
            }

            if (GUILayout.Button("添加配置"))
            {
                AddConfig();
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

        protected void DisplayRuntimeConfig(Transform child)
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
        protected void DisplayEditorConfig(Transform child)
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