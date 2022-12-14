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
        private string searchPath = "";
        private bool isNew = false;

        NodeGraphEditor nodeGraphEditor = null;

        private int selectNode = -1;
        private NodeType addNodeType = NodeType.Print;
        private object addNodePath = (SequencePath)1;

        private string luaFolder;

        [MenuItem("编辑工具/关卡流程图")]
        static void Init()
        {
            var window = GetWindow<NodeGraphWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            luaFolder = Application.dataPath + "/../LuaScripts/Public/Config/NodeGraphData/";
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.Width(1000)))
            {
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(300)))
                {
                    string fileText;
                    if (!string.IsNullOrEmpty(searchPath))
                    {
                        int folderIndex = searchPath.LastIndexOf("/");
                        if (folderIndex != -1)
                            fileText = searchPath.Substring(folderIndex + 1);
                        else
                            fileText = searchPath;
                    }
                    else if (isNew)
                        fileText = "新建配置";
                    else
                        fileText = "无";
                    EditorGUILayout.LabelField($"当前配置文件：{fileText}");

                    if (GUILayout.Button("新建配置文件"))
                    {
                        isNew = true;
                        searchPath = "";
                        NodeGraphData nodeGraphData = new NodeGraphData();
                        StartNode startNode = new StartNode();
                        startNode.nNodeId = 1;
                        nodeGraphData.AddNode(startNode);
                        nodeGraphData.SetStartNode(startNode);
                        nodeGraphEditor = new NodeGraphEditor(nodeGraphData, this);
                    }
                    if (GUILayout.Button("打开配置文件"))
                    {
                        searchPath = EditorUtility.OpenFilePanel("打开配置文件", luaFolder, "lua");
                        if (!string.IsNullOrEmpty(searchPath))
                        {
                            NodeGraphData nodeGraphData = SceneNodeGraphSerializer.LoadLua(searchPath);
                            nodeGraphEditor = new NodeGraphEditor(nodeGraphData, this);
                            isNew = false;
                        }
                    }
                    if (GUILayout.Button("保存配置文件"))
                    {
                        if (nodeGraphEditor != null)
                        {
                            bool bTips = true;
                            if (string.IsNullOrEmpty(searchPath))
                            {
                                bTips = false;
                                searchPath = EditorUtility.SaveFilePanel("选择保存文件", luaFolder, "New Graph", "lua");
                            }
                            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                            bool confirm = true;
                            if (bTips && File.Exists(searchPath))
                                if (!EditorUtility.DisplayDialog("提示", "将覆盖原有文件", "保存", "取消"))
                                    confirm = false;
                            if (confirm)
                                SceneNodeGraphSerializer.SaveLua(nodeGraphData, searchPath);
                        }
                        else
                            EditorUtility.DisplayDialog("提示", "没有配置文件", "确定");
                    }
                    if (GUILayout.Button("另存为..."))
                    {
                        if (nodeGraphEditor != null)
                        {
                            searchPath = EditorUtility.SaveFilePanel("选择保存文件", luaFolder, "", "lua");
                            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                            SceneNodeGraphSerializer.SaveLua(nodeGraphData, searchPath);
                        }
                        else
                            EditorUtility.DisplayDialog("提示", "没有配置文件", "确定");
                    }

                    if (nodeGraphEditor != null)
                    {
                        EditorGUILayout.LabelField("");
                        EditorGUILayout.LabelField($"当前选中节点： {selectNode}");
                        addNodeType = (NodeType)EditorGUILayout.EnumPopup("选择添加节点类型", addNodeType);
                        if (addNodePath.GetType() == typeof(int)) // CustomPath
                            addNodePath = EditorGUILayout.IntField("路径", (int)addNodePath);
                        else
                            addNodePath = EditorGUILayout.EnumPopup("路径", (Enum)addNodePath);
                        if (GUILayout.Button("添加子节点"))
                        {
                            if (selectNode > 0)
                                nodeGraphEditor.AddNode(selectNode, addNodeType, (int)addNodePath);
                            else
                                EditorUtility.DisplayDialog("提示", "没有选中节点", "确定");
                        }
                        if (GUILayout.Button("删除节点"))
                        {
                            if (selectNode > 0)
                            {
                                nodeGraphEditor.RemoveNode(selectNode);
                                selectNode = -1;
                            }
                            else
                                EditorUtility.DisplayDialog("提示", "没有选中节点", "确定");
                        }
                        EditorGUILayout.LabelField("");
                        NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
                        BaseNode nodeData = nodeGraphData.GetNodeData(selectNode);
                        EditorGUILayout.LabelField("节点输入信息：");
                        DrawNodeInputData(selectNode);
                        EditorGUILayout.LabelField("节点属性配置：");
                        NodeDrawer.DrawAttribute(nodeData, "nNodeId");
                    }

                }
                if (nodeGraphEditor != null)
                    nodeGraphEditor.Draw();
            }
        }

        private void DrawNodeInputData(int selectNode)
        {
            NodeGraphData nodeGraphData = nodeGraphEditor.GetNodeGraphData();
            BaseNode nodeData = nodeGraphData.GetNodeData(selectNode);
            if (nodeData == null) return;
            Type type = nodeData.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            bool hasInput = false;
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                NodeInputAttribute nodeInputAttribute = fieldInfo.GetCustomAttribute<NodeInputAttribute>();
                if (nodeInputAttribute != null)
                {
                    hasInput = true;
                    EditorGUILayout.LabelField($"{fieldInfo.Name}：");
                    if (!nodeGraphData.inputData.ContainsKey(selectNode))
                        nodeGraphData.inputData[selectNode] = new Dictionary<string, NodeInputData>();
                    if (!nodeGraphData.inputData[selectNode].ContainsKey(fieldInfo.Name))
                        nodeGraphData.inputData[selectNode][fieldInfo.Name] = new NodeInputData();
                    NodeInputData nodeInputData = nodeGraphData.inputData[selectNode][fieldInfo.Name];
                    nodeInputData.nodeId = EditorGUILayout.IntField(" 来源节点编号", nodeInputData.nodeId);
                    nodeInputData.attrName = EditorGUILayout.TextField(" 来源节点输出属性", nodeInputData.attrName);
                }
            }
            if (!hasInput)
                EditorGUILayout.LabelField("无");
        }

        public void OnNodeSelect(int nodeId)
        {
            if (nodeId == selectNode)
                return;
            selectNode = nodeId;
            BaseNode currNode = nodeGraphEditor.GetNodeGraphData().GetNodeData(selectNode);
            if (currNode != null)
            {
                addNodePath = 1;
                Type pathType = currNode.GetPathType();
                if(pathType != typeof(CustomPath))
                {
                    foreach (var element in Enum.GetValues(pathType))
                    {
                        if ((int)addNodePath == (int)element)
                        {
                            addNodePath = element;
                        }
                    }
                }
            }
        }
    }
}
