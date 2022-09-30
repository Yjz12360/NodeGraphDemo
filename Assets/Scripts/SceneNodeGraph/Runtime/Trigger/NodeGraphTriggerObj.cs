using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace SceneNodeGraph
{
    public class NodeGraphTriggerObj : MonoBehaviour
    {
        // Start is called before the first frame update
        public NodeGraphTrigger sourceTrigger;
        public bool bTriggered = false;

        private void OnTriggerEnter(Collider collider)
        {
            if (sourceTrigger == null) return;
            if (bTriggered) return;
            if(collider.gameObject.GetComponent<Game.PlayerTriggerSign>() != null)
            {
                sourceTrigger.OnTrigger(collider);
            }
        }


    }

}

