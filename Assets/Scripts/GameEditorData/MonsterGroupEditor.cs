using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(MonsterGroupEditorData))]
    public class MonsterGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MonsterGroupEditorData data = target as MonsterGroupEditorData;
            Transform configTrans = data.transform.parent.Find("Config/MonsterGroup");

            EditorGUILayout.LabelField("当前怪物组配置：");
            for (int i = 0; i < configTrans.childCount; ++i)
            {
                DisplayMonsterGroupConfig(configTrans, i);
            }
            if (GUILayout.Button("添加怪物组配置"))
            {
                int genId = GenId(configTrans);
                GameObject configObject = new GameObject(genId.ToString());

                configObject.transform.SetParent(configTrans);
                configObject.transform.position = Vector3.zero;
                configObject.AddComponent<MonsterGroupConfig>();
                EditorGUIUtility.PingObject(configObject);
            }
        }

        private void DisplayMonsterGroupConfig(Transform configTrans, int childIndex)
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