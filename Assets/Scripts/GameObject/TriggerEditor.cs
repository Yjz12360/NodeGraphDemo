﻿using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TriggerEditorData))]
    public class TriggerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            GameObject triggerContainer = GameObject.Find("GameObjects/Triggers");
            Transform containerTrans = triggerContainer.transform;

            EditorGUILayout.LabelField("当前触发器：");
            for(int i = 0; i < containerTrans.childCount; ++i)
            {
                DisplayTrigger(containerTrans, i);
            }
            if (GUILayout.Button("添加触发器"))
            {
                int genId = GenTriggerId();
                //GameObject newObject = new GameObject(genId.ToString());
                GameObject prefab = Resources.Load<GameObject>("GamePrefabs/Trigger/Trigger");
                GameObject newObject = Instantiate(prefab);
                newObject.name = genId.ToString();
                LuaOnTrigger luaOnTrigger = newObject.GetComponent<LuaOnTrigger>();
                if (luaOnTrigger != null)
                    luaOnTrigger.triggerId = genId;
                newObject.transform.SetParent(containerTrans);
                newObject.transform.position = Vector3.zero;
                BoxCollider collider = newObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
                collider.center = Vector3.zero;
                EditorGUIUtility.PingObject(newObject);
            }
        }

        private void DisplayTrigger(Transform containerTrans, int childIndex)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                Transform child = containerTrans.GetChild(childIndex);
                EditorGUILayout.ObjectField(child.gameObject, typeof(GameObject), true);
                if (GUILayout.Button("删除"))
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }

        private int GenTriggerId()
        {
            GameObject triggerContainer = GameObject.Find("GameObjects/Triggers");
            Transform containerTrans = triggerContainer.transform;
            for (int i = 1; i < 10000; ++i)
            {
                Transform child = containerTrans.Find(i.ToString());
                if(child == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}