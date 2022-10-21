using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;

namespace SceneNodeGraph
{
    class NodeGraphWindow : EditorWindow
    {
        private string sSearchPath = "";
        private bool bIsNew = false;

        NodeGraphEditor nodeGraphEditor = null;

        private string sSelectNode = "";
        private NodeType nAddNodeType = NodeType.Print;
        private int nAddNodePath = 1;

        private string sLuaFolder;

        [MenuItem("编辑工具/关卡流程图")]
        static void Init()
        {
            var window = GetWindow<NodeGraphWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            sLuaFolder = Application.dataPath + "/../LuaScripts/Public/Config/NodeGraphData/";
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.Width(1000)))
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(200)))
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



                    if (GUILayout.Button("新建配置文件"))
                    {
                        bIsNew = true;
                        sSearchPath = "";
                        NodeGraphData nodeGraphData = new NodeGraphData();
                        BaseNode startNode = new BaseNode();
                        startNode.sNodeId = "1";
                        nodeGraphData.AddNode(startNode);
                        nodeGraphData.SetStartNode(startNode);
                        nodeGraphEditor = new NodeGraphEditor(nodeGraphData, this);
                    }
                    if (GUILayout.Button("打开配置文件"))
                    {
                        sSearchPath = EditorUtility.OpenFilePanel("打开配置文件", sLuaFolder, "lua");
                        if (!string.IsNullOrEmpty(sSearchPath))
                        {
                            NodeGraphData nodeGraphData = SceneNodeGraphSerializer.LoadLua(sSearchPath);
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
                                sSearchPath = EditorUtility.SaveFilePanel("选择保存文件", sLuaFolder, "New Graph", "lua");
                            }
                            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                            bool bConfirm = true;
                            if (File.Exists(sSearchPath))
                                if (!EditorUtility.DisplayDialog("提示", "将覆盖原有文件", "保存", "取消"))
                                    bConfirm = false;
                            if (bConfirm)
                                SceneNodeGraphSerializer.SaveLua(nodeGraphData, sSearchPath);
                        }
                        else
                            EditorUtility.DisplayDialog("提示", "没有配置文件", "确定");
                    }
                    if (GUILayout.Button("另存为..."))
                    {
                        if (nodeGraphEditor != null)
                        {
                            sSearchPath = EditorUtility.SaveFilePanel("选择保存文件", sLuaFolder, "", "lua");
                            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                            SceneNodeGraphSerializer.SaveLua(nodeGraphData, sSearchPath);
                        }
                        else
                            EditorUtility.DisplayDialog("提示", "没有配置文件", "确定");
                    }

                    if (nodeGraphEditor != null)
                    {
                        EditorGUILayout.LabelField("");
                        EditorGUILayout.LabelField($"当前选中节点： {sSelectNode}");
                        nAddNodeType = (NodeType)EditorGUILayout.EnumPopup("选择添加节点类型", nAddNodeType);
                        nAddNodePath = EditorGUILayout.IntField("路径编号", nAddNodePath);
                        if (GUILayout.Button("添加子节点"))
                        {
                            if (!string.IsNullOrEmpty(sSelectNode))
                                nodeGraphEditor.AddNode(sSelectNode, nAddNodeType, nAddNodePath);
                            else
                                EditorUtility.DisplayDialog("提示", "没有选中节点", "确定");
                        }
                        if (GUILayout.Button("删除节点"))
                        {
                            if (!string.IsNullOrEmpty(sSelectNode))
                                nodeGraphEditor.RemoveNode(sSelectNode);
                            else
                                EditorUtility.DisplayDialog("提示", "没有选中节点", "确定");
                        }
                        EditorGUILayout.LabelField("");
                        EditorGUILayout.LabelField("节点属性配置");
                        NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                        BaseNode nodeData = nodeGraphData.GetNodeData(sSelectNode);
                        if (nodeData != null)
                        {
                            Type type = nodeData.GetType();
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
                                    else if (fieldType == typeof(bool))
                                    {
                                        bool bOldValue = (bool)fieldInfo.GetValue(nodeData);
                                        bool bNewValue = EditorGUILayout.Toggle(fieldInfo.Name, bOldValue);
                                        if (bNewValue != bOldValue)
                                            fieldInfo.SetValue(nodeData, bNewValue);
                                    }
                                    else if (fieldType == typeof(Enum))
                                    {
                                        Enum oldValue = (Enum)fieldInfo.GetValue(nodeData);
                                        Enum newValue = EditorGUILayout.EnumPopup(fieldInfo.Name, oldValue);
                                        if (newValue != oldValue)
                                            fieldInfo.SetValue(nodeData, newValue);
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
