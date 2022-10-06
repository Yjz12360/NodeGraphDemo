using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace SceneNodeGraph
{
    public class NodeGraphTriggerObj : MonoBehaviour
    {
        public NodeGraphTrigger sourceTrigger;
        public bool bTriggered = false;

        private void OnTriggerEnter(Collider collider)
        {
            if (sourceTrigger == null) return;
            if (bTriggered) return;
            if(collider.gameObject.GetComponent<Game.PlayerCollider>() != null)
            {
                sourceTrigger.OnTrigger(collider);
            }
        }


    }

}

