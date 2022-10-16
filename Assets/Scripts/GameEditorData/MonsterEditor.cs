using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(MonsterEditorData))]
    public class MonsterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameObject configContainer = GameObject.Find("Config/Monster");
            Transform configTrans = configContainer.transform;

            EditorGUILayout.LabelField("当前怪物配置：");
            for (int i = 0; i < configTrans.childCount; ++i)
            {
                DisplayMonsterConfig(configTrans, i);
            }
            if (GUILayout.Button("添加怪物配置"))
            {
                int genId = GenId(configContainer);
                GameObject configObject = new GameObject(genId.ToString());

                configObject.transform.SetParent(configTrans);
                configObject.transform.position = Vector3.zero;
                ConfigId configId = configObject.AddComponent<ConfigId>();
                PathEditorData pathEditorData = configObject.AddComponent<PathEditorData>();
                EditorGUIUtility.PingObject(configObject);
            }
        }

        private void DisplayMonsterConfig(Transform configTrans, int childIndex)
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

        private int GenId(GameObject configContainer)
        {
            Transform containerTrans = configContainer.transform;
            for (int i = 1; i < 10000; ++i)
            {
                Transform child = containerTrans.Find(i.ToString());
                if (child == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}