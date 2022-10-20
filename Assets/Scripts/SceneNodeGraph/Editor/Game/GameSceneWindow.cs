using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using UnityEditor.SceneManagement;

namespace Game
{
    public class GameSceneWindow : EditorWindow
    {
        private string configName;
        private GameObject editorObject;
        private GameObject sceneDataObject;

        [MenuItem("编辑工具/游戏物体配置")]
        static void Init()
        {
            var window = GetWindow<GameSceneWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            configName = EditorSceneManager.GetActiveScene().name;
        }


        private void OnGUI()
        {
            configName = EditorGUILayout.TextField("场景配置名", configName);
            if (GUILayout.Button("加载编辑器"))
            {
                DoLoad();
                EditorGUIUtility.PingObject(editorObject);
                
            }
            if (GUILayout.Button("保存配置"))
            {
                if (editorObject != null)
                {
                    if (sceneDataObject != null)
                    {
                        string prefabPath = $"Assets/Resources/SceneData/{configName}.prefab";
                        PrefabUtility.SaveAsPrefabAsset(sceneDataObject, prefabPath);
                    }

                    Transform configTrans = editorObject.transform.Find("Config");
                    if (configTrans != null)
                    {
                        GameConfigSerializer.SaveLua(configTrans.gameObject, configName);
                    }
                    DestroyImmediate(editorObject);

                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    DoLoad();
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "请先加载编辑器配置", "确认");
                }
            }
            if (GUILayout.Button("删除编辑器"))
            {
                if (editorObject != null)
                    DestroyImmediate(editorObject);
            }
        }

        private void DoLoad()
        {
            if (editorObject != null)
                DestroyImmediate(editorObject);
            GameObject editorPrefab = Resources.Load<GameObject>("GamePrefabs/Editor");
            editorObject = Instantiate(editorPrefab);
            editorObject.name = editorPrefab.name;
            GameObject configObject = GameConfigSerializer.LoadLua(configName);
            if (configObject == null)
                configObject = new GameObject("Config");
            configObject.transform.SetParent(editorObject.transform);

            GameObject sceneDataPrefab;
            if(File.Exists($"{Application.dataPath}/Resources/SceneData/{configName}.prefab"))
                sceneDataPrefab = Resources.Load<GameObject>($"SceneData/{configName}");
            else
                sceneDataPrefab = Resources.Load<GameObject>("SceneData/Empty");

            sceneDataObject = Instantiate(sceneDataPrefab);
            sceneDataObject.transform.parent = editorObject.transform.Find("SceneData"); 
        }

    }
}
