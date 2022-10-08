using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace Game
{
    public class NodeGraphTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        public Collider checkCollider;

        private CltObjectData cltObjectData;

        public void Start()
        {
            cltObjectData = gameObject.GetComponent<CltObjectData>();

            GameObject targetObject = checkCollider != null ? checkCollider.gameObject : gameObject;
            NodeGraphTriggerObj triggerObj = targetObject.AddComponent<NodeGraphTriggerObj>();
            triggerObj.sourceTrigger = this;
        }
        public void OnTrigger(GameObject playerObject)
        {
            CltObjectData objectData = playerObject.GetComponent<CltObjectData>();
            int nPlayerId = objectData.nGameObjectId;
            int nObjectId = cltObjectData.nGameObjectId;
            GameMessager.C2SActivateTrigger(nPlayerId, nObjectId);
        }

        public void Destroy()
        {
            GameObject targetObject = checkCollider != null ? checkCollider.gameObject : gameObject;
            NodeGraphTriggerObj triggerObj = targetObject.GetComponent<NodeGraphTriggerObj>();
            if(triggerObj != null && triggerObj.sourceTrigger == this)
            {
                Destroy(triggerObj);
            }
        }
    }

}

