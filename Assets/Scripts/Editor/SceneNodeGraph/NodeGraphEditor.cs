using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SceneNodeGraph
{
    class NodeGraphEditor
    {
        private static readonly float drawOffsetX = 200;
        private static readonly float drawOffsetY = 40;
        private static readonly float padding = 20;
        private static readonly float nodeWidth = 160;
        private static readonly float nodeHeight = 40;
        private static readonly float pathTextWidth = 16;
        private static readonly float pathTextHeight = 20;

        private static Texture2D backgroundTexture = null;

        private NodeGraphWindow window;
        private NodeGraphData nodeGraphData;

        private Dictionary<int, Vector2Int> nodeCoords = new Dictionary<int, Vector2Int>();
        public NodeGraphEditor(NodeGraphData nodeGraphData, NodeGraphWindow window)
        {
            this.nodeGraphData = nodeGraphData;
            this.window = window;
            RefreshNodes();
        }

        public NodeGraphData GetNodeGraphData()
        {
            return nodeGraphData;
        }

        public void RefreshNodes()
        {
            nodeCoords.Clear();
            nodeCoords[nodeGraphData.startNodeId] = new Vector2Int(0, 0);
            RecursiveCalCoord(nodeGraphData.startNodeId);
        }

        private int RecursiveCalCoord(int rootNodeId)
        {
            int coordOffsetY = 0;
            if(nodeGraphData.transitions.ContainsKey(rootNodeId))
            {
                Vector2Int coord = nodeCoords[rootNodeId];
                bool first = true;
                foreach(var nodeTransitions in nodeGraphData.transitions[rootNodeId])
                {
                    foreach(int toNodeId in nodeTransitions.Value)
                    {
                        if (nodeCoords.ContainsKey(toNodeId))
                            continue;
                        if (!first)
                            coordOffsetY++;
                        first = false;
                        nodeCoords[toNodeId] = new Vector2Int(coord.x + 1, coord.y + coordOffsetY);
                        int offsetY = RecursiveCalCoord(toNodeId);
                        coordOffsetY += offsetY;
                    }
                }
            }
            return coordOffsetY;
        }

        public void AddNode(int fromNodeId, NodeType nodeType, int path)
        {
            if (fromNodeId <= 0)
                return;
            int newNodeId = -1;
            for (int i = 1; i < 100; ++i)
            {
                if (!nodeGraphData.nodeMap.ContainsKey(i))
                {
                    newNodeId = i;
                    break;
                }
            }
            Type type = BaseNode.GetType(nodeType);
            if (type == null)
                type = typeof(BaseNode);
            BaseNode nodeData = (BaseNode)Activator.CreateInstance(type);
            nodeData.nNodeId = newNodeId;
            nodeGraphData.AddNode(nodeData);
            nodeGraphData.AddTransition(fromNodeId, newNodeId, path);
            RefreshNodes();
        }

        public void RemoveNode(int nodeId)
        {
            nodeGraphData.RemoveNode(nodeId);
            RefreshNodes();
        }

        public void Draw()
        {
            DrawBackground();
            DrawNodes();
            DrawTransitions();
        }
        private Vector2 GetDrawPos(Vector2Int coord)
        {
            float x = drawOffsetX + nodeWidth * (coord.x * 1.5f + 0.5f);
            float y = drawOffsetY + nodeHeight * (coord.y * 1.5f + 0.5f);
            return new Vector2(x, y);
        }

        private void DrawBackground()
        {
            if (backgroundTexture == null)
            {
                backgroundTexture = new Texture2D(2000, 1000);
                for (int i = 0; i < backgroundTexture.width; ++i)
                    for (int j = 0; j < backgroundTexture.height; ++j)
                        backgroundTexture.SetPixel(i, j, Color.gray);
                backgroundTexture.Apply();
            }
            GUI.DrawTexture(new Rect(drawOffsetX + padding, padding, 2000, 1000), backgroundTexture);
        }

        private void DrawNodes()
        {
            foreach (var pair in nodeCoords)
            {
                int nodeId = pair.Key;
                string context;
                if (nodeId == nodeGraphData.startNodeId)
                    context = "开始节点";
                else
                {
                    string nodeType = nodeGraphData.nodeMap[nodeId].GetNodeType().ToString();
                    context = $"{nodeId}\n{nodeType}";
                }
                Vector2 drawPos = GetDrawPos(pair.Value);
                if (GUI.Button(new Rect(drawPos.x, drawPos.y, nodeWidth, nodeHeight), context))
                    window.OnNodeSelect(nodeId);
            }
        }
        private void DrawTransitions()
        {
            foreach(var transitionPair in nodeGraphData.transitions)
            {
                int fromNodeId = transitionPair.Key;
                foreach(var transitionData in transitionPair.Value)
                {
                    int path = transitionData.Key;
                    foreach(int toNodeId in transitionData.Value)
                    {
                        Vector2 fromPos = GetDrawPos(nodeCoords[fromNodeId]);
                        fromPos.x += nodeWidth;
                        fromPos.y += nodeHeight / 2;
                        Vector2 toPos = GetDrawPos(nodeCoords[toNodeId]);
                        toPos.y += nodeHeight / 2;
                        Handles.DrawLine(fromPos, toPos);
                        Vector2 pathTextPos = (fromPos + toPos) / 2;
                        pathTextPos.x -= pathTextWidth / 2;
                        pathTextPos.y -= pathTextHeight / 2;
                        using (new EditorGUI.DisabledScope())
                            GUI.TextField(new Rect(pathTextPos.x, pathTextPos.y, pathTextWidth, pathTextHeight), path.ToString());
                    }
                }
            }
        }
    }
}
