using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [CustomEditor(typeof(TransparentWallEditorData))]
    public class TransparentWallEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameObject sceneData = GameObject.Find("Editor/SceneData");
            if (sceneData == null)
                return;
            Transform configTrans = sceneData.transform.GetChild(0);
            if (configTrans == null)
                return;
            Transform containerTrans = configTrans.Find("TransparentWall");
            if (containerTrans != null)
            {
                EditorGUILayout.LabelField("当前空气墙：");
                for (int i = 0; i < containerTrans.childCount; ++i)
                {
                    DisplayTrigger(containerTrans, i);
                }
            }
            if (GUILayout.Button("添加空气墙"))
            {
                if(containerTrans == null)
                {
                    GameObject containerObject = new GameObject("TransparentWall");
                    containerTrans = containerObject.transform;
                    containerTrans.transform.parent = configTrans;
                }
                int genId = GenTriggerId();
                GameObject newObject = new GameObject(genId.ToString());
                newObject.name = genId.ToString();
                newObject.transform.SetParent(containerTrans);
                newObject.transform.position = Vector3.zero;
                BoxCollider collider = newObject.GetComponent<BoxCollider>();
                if (collider == null)
                    collider = newObject.AddComponent<BoxCollider>();
                collider.isTrigger = false;
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
            GameObject sceneData = GameObject.Find("Editor/SceneData");
            if (sceneData == null)
                return 0;
            Transform configTrans = sceneData.transform.GetChild(0);
            if (configTrans == null)
                return 0;
            Transform containerTrans = configTrans.Find("TransparentWall");
            if (containerTrans == null)
                return 0;
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

        //private Transform GetWallContainer()
        //{
        //    GameObject sceneData = GameObject.Find("Editor/SceneData");
        //    if (sceneData == null)
        //        return null;
        //    Transform configTrans = sceneData.transform.GetChild(0);
        //    if (configTrans == null)
        //        return null;
        //    return configTrans.Find("TransparentWall");
        //}
    }
}