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
        private static float nDrawOffsetX = 200;
        private static float nDrawOffsetY = 40;
        private static float nPadding = 20;
        private static float nNodeWidth = 160;
        private static float nNodeHeight = 40;
        private static float nPathTextWidth = 16;
        private static float nPathTextHeight = 20;

        private static Texture2D backgroundTexture = null;

        private NodeGraphWindow window;
        private NodeGraphData nodeGraphData;

        private Dictionary<string, Vector2Int> tNodeCoords = new Dictionary<string, Vector2Int>();
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
            tNodeCoords.Clear();
            tNodeCoords[nodeGraphData.sStartNodeId] = new Vector2Int(0, 0);
            RecursiveCalCoord(nodeGraphData.sStartNodeId);
        }

        private int RecursiveCalCoord(string sRootNodeId)
        {
            int nCoordOffsetY = 0;
            Vector2Int coord = tNodeCoords[sRootNodeId];
            bool bFirst = true;
            foreach (NodeTransitionData transition in nodeGraphData.tTransitions)
            {
                if (transition.sFromNodeId == sRootNodeId)
                {
                    string sToNodeId = transition.sToNodeId;
                    if (tNodeCoords.ContainsKey(sToNodeId))
                        continue;
                    if (!bFirst)
                        nCoordOffsetY++;
                    bFirst = false;
                    tNodeCoords[sToNodeId] = new Vector2Int(coord.x + 1, coord.y + nCoordOffsetY);
                    int nOffsetY = RecursiveCalCoord(sToNodeId);
                    nCoordOffsetY += nOffsetY;
                }
            }
            return nCoordOffsetY;
        }

        public void AddNode(string sFromNodeId, NodeType nNodeType, int nPath)
        {
            if (string.IsNullOrEmpty(sFromNodeId))
                return;
            string sNewNodeId = "";
            for (int i = 1; i < 100; ++i)
            {
                if (!nodeGraphData.tNodeMap.ContainsKey(i.ToString()))
                {
                    sNewNodeId = i.ToString();
                    break;
                }
            }
            Type type = BaseNodeData.GetType(nNodeType);
            if (type == null)
                type = typeof(BaseNodeData);
            BaseNodeData nodeData = (BaseNodeData)Activator.CreateInstance(type);
            nodeData.sNodeId = sNewNodeId;
            nodeGraphData.AddNode(nodeData);
            nodeGraphData.AddTransition(sFromNodeId, sNewNodeId, nPath);
            RefreshNodes();
        }

        public void RemoveNode(string sNodeId)
        {
            nodeGraphData.RemoveNode(sNodeId);
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
            float x = nDrawOffsetX + nNodeWidth * (coord.x * 1.5f + 0.5f);
            float y = nDrawOffsetY + nNodeHeight * (coord.y * 1.5f + 0.5f);
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
            GUI.DrawTexture(new Rect(nDrawOffsetX + nPadding, nPadding, 2000, 1000), backgroundTexture);
        }

        private void DrawNodes()
        {
            foreach (var pair in tNodeCoords)
            {
                string sNodeId = pair.Key;
                string sContext;
                if (sNodeId == nodeGraphData.sStartNodeId)
                    sContext = "开始节点";
                else
                {
                    string sNodeType = nodeGraphData.tNodeMap[sNodeId].GetNodeType().ToString();
                    sContext = $"{sNodeId}\n{sNodeType}";
                }
                Vector2 drawPos = GetDrawPos(pair.Value);
                if (GUI.Button(new Rect(drawPos.x, drawPos.y, nNodeWidth, nNodeHeight), sContext))
                    window.OnNodeSelect(sNodeId);
            }
        }
        private void DrawTransitions()
        {
            foreach (NodeTransitionData transition in nodeGraphData.tTransitions)
            {
                string sFromNodeId = transition.sFromNodeId;
                string sToNodeId = transition.sToNodeId;
                Vector2 fromPos = GetDrawPos(tNodeCoords[sFromNodeId]);
                fromPos.x += nNodeWidth;
                fromPos.y += nNodeHeight / 2;
                Vector2 toPos = GetDrawPos(tNodeCoords[sToNodeId]);
                toPos.y += nNodeHeight / 2;
                Handles.DrawLine(fromPos, toPos);
                Vector2 pathTextPos = (fromPos + toPos) / 2;
                pathTextPos.x -= nPathTextWidth / 2;
                pathTextPos.y -= nPathTextHeight / 2;
                string sPath = transition.nPath.ToString();
                using (new EditorGUI.DisabledScope())
                    GUI.TextField(new Rect(pathTextPos.x, pathTextPos.y, nPathTextWidth, nPathTextHeight), sPath);
            }
        }
    }
}
