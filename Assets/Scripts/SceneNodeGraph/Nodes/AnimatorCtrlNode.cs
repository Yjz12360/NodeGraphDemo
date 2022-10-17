using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class AnimatorCtrlNodeData : BaseNodeData
    {
        public override NodeType GetNodeType() { return NodeType.AnimatorCtrl; }

        public int nStaticId;
        public string sSetTrigger;
        //public string sAnimName;
    }

    //public class CltAnimatorCtrlNode : CltRuntimeNode
    //{
    //    public AnimatorCtrlNodeData NodeData { get { return (AnimatorCtrlNodeData)baseNodeData; } }

    //    public override void StartNode()
    //    {
    //        if (!string.IsNullOrEmpty(NodeData.sSetTrigger))
    //        {
    //            GameObject monsterObject = nodeGraph.game.GetMonsterByStaticId(NodeData.nStaticId);
    //            if (monsterObject != null)
    //            {
    //                Game.MonsterControl monsterControl = monsterObject.GetComponent<Game.MonsterControl>();
    //                //monsterControl.modelAnimator.Play(NodeData.sAnimName);
    //                monsterControl.modelAnimator.SetTrigger(NodeData.sSetTrigger);
    //            }
    //        }

    //        FinishNode();
    //    }
    //}

}
