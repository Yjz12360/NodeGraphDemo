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
    class GameSceneWindow : EditorWindow
    {
        private GameObject editorObject;

        [MenuItem("编辑工具/游戏物体配置")]
        static void Init()
        {
            var window = GetWindow<GameSceneWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("加载编辑器"))
            {
                LoadConfigObject();
                EditorGUIUtility.PingObject(editorObject);
                
            }
            if (GUILayout.Button("保存配置"))
            {
                if(editorObject != null)
                {
                    Transform configTrans = editorObject.transform.Find("Config");
                    if(configTrans != null)
                    {
                        string configName = EditorSceneManager.GetActiveScene().name;
                        GameConfigSerializer.SaveLua(configTrans.gameObject, configName);
                    }
                    DestroyImmediate(editorObject);
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                    LoadConfigObject();
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

        private void LoadConfigObject()
        {
            if (editorObject != null)
                DestroyImmediate(editorObject);
            GameObject editorPrefab = Resources.Load<GameObject>("GamePrefabs/Editor");
            editorObject = Instantiate(editorPrefab);
            editorObject.name = editorPrefab.name;
            string configName = EditorSceneManager.GetActiveScene().name;
            GameObject configObject = GameConfigSerializer.LoadLua(configName);
            configObject.transform.SetParent(editorObject.transform);
        }
    }
}
