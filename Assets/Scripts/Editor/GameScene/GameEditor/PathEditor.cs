using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(PathEditorData))]
    public class PathEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PathEditorData pathEditorData = target as PathEditorData;
            Transform monsterTrans = pathEditorData.gameObject.transform;
            Transform pathContainerTrans = GameEditorHelper.GetOrAddChild(monsterTrans, "Path");

            EditorGUILayout.LabelField("当前路径配置：");
            for (int i = 0; i < pathContainerTrans.childCount; ++i)
                DisplayPathConfig(pathContainerTrans.GetChild(i));
            
            if (GUILayout.Button("添加路径配置"))
            {
                GameObject configObject = GameEditorHelper.AddConfig(pathContainerTrans);
                EditorGUIUtility.PingObject(configObject);
            }
        }

        private void DisplayPathConfig(Transform child)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
                if (GUILayout.Button("删除"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}