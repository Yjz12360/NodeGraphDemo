using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(MonsterEditorData))]
    public class MonsterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MonsterEditorData data = target as MonsterEditorData;
            Transform configRoot = data.transform.parent.Find("Config");
            if (configRoot == null)
                return;
            Transform configTrans = configRoot.Find("Monster");
            if (configTrans != null && configTrans.childCount > 0)
            {
                EditorGUILayout.LabelField("当前怪物配置：");
                for (int i = 0; i < configTrans.childCount; ++i)
                {
                    DisplayMonsterConfig(configTrans, i);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("当前没有配置怪物", MessageType.Info);
            }

            if (GUILayout.Button("添加怪物配置"))
            {
                if (configTrans == null)
                {
                    GameObject newObject = new GameObject("Monster");
                    newObject.transform.parent = configRoot;
                    configTrans = newObject.transform;
                }

                int genId = GenId(configTrans);
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

        private int GenId(Transform containerTrans)
        {
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