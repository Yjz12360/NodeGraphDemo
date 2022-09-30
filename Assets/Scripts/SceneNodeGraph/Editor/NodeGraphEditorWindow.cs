﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace SceneNodeGraph
{
    class NodeGraphEditorWindow : EditorWindow
    {
        private string sSearchPath = "";
        private bool bIsNew = false;

        NodeGraphEditor nodeGraphEditor = null;

        private string sSelectNode = "";
        private NodeType nAddNodeType = NodeType.Print;

        [MenuItem("节点图编辑器/打开")]
        static void Init()
        {
            var window = GetWindow<NodeGraphEditorWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.Width(1000)))
            {
                using(new EditorGUILayout.VerticalScope(GUILayout.Width(200)))
                {
                    string sFile;
                    if (!string.IsNullOrEmpty(sSearchPath))
                    {
                        int nFolderIndex = sSearchPath.LastIndexOf("/");
                        if (nFolderIndex != -1)
                            sFile = sSearchPath.Substring(nFolderIndex + 1);
                        else
                            sFile = sSearchPath;
                    }
                    else if (bIsNew)
                        sFile = "新建配置";
                    else
                        sFile = "无";
                    EditorGUILayout.LabelField($"当前配置文件：{sFile}");

                    string sFolder = Application.dataPath + "/NodeGraphData/";
                    if (GUILayout.Button("新建配置文件"))
                    {
                        bIsNew = true;
                        sSearchPath = "";
                        NodeGraphData nodeGraphData = new NodeGraphData();
                        BaseNodeData startNode = new BaseNodeData();
                        startNode.sNodeId = "1";
                        nodeGraphData.AddNode(startNode);
                        nodeGraphData.SetStartNode(startNode);
                        nodeGraphEditor = new NodeGraphEditor(nodeGraphData, this);
                    }
                    if (GUILayout.Button("打开配置文件"))
                    {
                        sSearchPath = EditorUtility.OpenFilePanel("打开配置文件", sFolder, "json");
                        if (!string.IsNullOrEmpty(sSearchPath))
                        {
                            NodeGraphData nodeGraphData = SceneNodeGraphSerializer.LoadPath(sSearchPath);
                            nodeGraphEditor = new NodeGraphEditor(nodeGraphData, this);
                            bIsNew = false;
                        }
                    }
                    if (GUILayout.Button("保存配置文件"))
                    {
                        if (nodeGraphEditor != null)
                        {
                            if (string.IsNullOrEmpty(sSearchPath))
                            {
                                sSearchPath = EditorUtility.SaveFilePanel("选择保存文件", sFolder, "New Graph", "json");
                            }
                            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                            SceneNodeGraphSerializer.SavePath(nodeGraphData, sSearchPath);
                        }
                        else
                            EditorUtility.DisplayDialog("提示", "没有配置文件", "确定");
                    }

                    if(nodeGraphEditor != null)
                    {
                        EditorGUILayout.LabelField("");
                        EditorGUILayout.LabelField($"当前选中节点： {sSelectNode}");
                        nAddNodeType = (NodeType)EditorGUILayout.EnumPopup("选择添加节点类型", nAddNodeType);
                        if (GUILayout.Button("添加子节点"))
                        {
                            nodeGraphEditor.AddNode(sSelectNode, nAddNodeType);
                        }
                        EditorGUI.BeginDisabledGroup(true);
                        if (GUILayout.Button("删除节点"))
                        {
                            nodeGraphEditor.RemoveNode(sSelectNode);
                        }
                        EditorGUI.EndDisabledGroup();

                        EditorGUILayout.LabelField("");
                        EditorGUILayout.LabelField("节点属性配置");
                        NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                        BaseNodeData nodeData = nodeGraphData.GetNodeData(sSelectNode);
                        if(nodeData != null)
                        {
                            Type type = nodeData.Type;
                            if (type != null)
                            {
                                FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                                foreach (FieldInfo fieldInfo in fieldInfos)
                                {
                                    if (fieldInfo.Name == "sNodeId") continue;
                                    Type fieldType = fieldInfo.FieldType;
                                    if (fieldType == typeof(int))
                                    {
                                        int nOldValue = (int)fieldInfo.GetValue(nodeData);
                                        int nNewValue = EditorGUILayout.IntField(fieldInfo.Name, nOldValue);
                                        if (nNewValue != nOldValue)
                                            fieldInfo.SetValue(nodeData, nNewValue);
                                    }
                                    else if (fieldType == typeof(float))
                                    {
                                        float nOldValue = (float)fieldInfo.GetValue(nodeData);
                                        float nNewValue = EditorGUILayout.FloatField(fieldInfo.Name, nOldValue);
                                        if (nNewValue != nOldValue)
                                            fieldInfo.SetValue(nodeData, nNewValue);
                                    }
                                    else if (fieldType == typeof(string))
                                    {
                                        string sOldValue = (string)fieldInfo.GetValue(nodeData);
                                        if (sOldValue == null) sOldValue = "";
                                        string sNewValue = EditorGUILayout.TextField(fieldInfo.Name, sOldValue);
                                        if (!sNewValue.Equals(sOldValue))
                                            fieldInfo.SetValue(nodeData, sNewValue);
                                    }
                                }
                            }
                        }
                    }
 
                }
                if (nodeGraphEditor != null)
                    nodeGraphEditor.Draw();
            }
        }

        public void OnNodeSelect(string sNodeId)
        {
            sSelectNode = sNodeId;
        }
    }
}