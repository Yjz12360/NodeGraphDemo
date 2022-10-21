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
            Transform pathContainerTrans = monsterTrans.Find("Path");
            if (pathContainerTrans == null)
            {
                GameObject pathObject = new GameObject("Path");
                pathObject.transform.parent = monsterTrans;
                pathObject.transform.position = Vector3.zero;
                pathContainerTrans = pathObject.transform;
            }

            EditorGUILayout.LabelField("当前路径配置：");
            for (int i = 0; i < pathContainerTrans.childCount; ++i)
            {
                DisplayPathConfig(pathContainerTrans, i);
            }
            if (GUILayout.Button("添加路径配置"))
            {
                int genId = GenId(pathContainerTrans);
                GameObject configObject = new GameObject(genId.ToString());

                configObject.transform.SetParent(pathContainerTrans);
                configObject.transform.position = Vector3.zero;
                EditorGUIUtility.PingObject(configObject);
            }
        }

        private void DisplayPathConfig(Transform configTrans, int childIndex)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                Transform child = configTrans.GetChild(childIndex);
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
                if (GUILayout.Button("删除"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private int GenId(Transform pathTrans)
        {
            for (int i = 1; i < 10000; ++i)
            {
                Transform child = pathTrans.Find(i.ToString());
                if (child == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}