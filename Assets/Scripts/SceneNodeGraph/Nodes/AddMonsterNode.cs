using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AddMonsterNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.AddMonster; }

        public int nPrefabId;
        public int nHP;
        public float nPosX;
        public float nPosY;
        public float nPosZ;
    }

    public class SvrAddMonsterNode : SvrRuntimeNode
    {
        public override bool SyncFinishNode() { return true; }
        public AddMonsterNodeData NodeData { get { return (AddMonsterNodeData)baseNodeData; } }

        public override void StartNode()
        {
            float nPosX = NodeData.nPosX;
            float nPosY = NodeData.nPosY;
            float nPosZ = NodeData.nPosZ;
            Game.MonsterConfigData configData = new Game.MonsterConfigData();
            configData.nPrefabId = NodeData.nPrefabId;
            configData.nHP = NodeData.nHP;
            nodeGraph.game.AddMonster(configData, new Vector3(nPosX, nPosY, nPosZ));
            FinishNode();
        }
    }
}
