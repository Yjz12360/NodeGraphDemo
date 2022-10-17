//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.TextCore;

//namespace Game
//{
//    public class NodeGraphTriggerObj : MonoBehaviour
//    {
//        public NodeGraphTrigger sourceTrigger;
//        public bool bTriggered = false;

//        private void OnTriggerEnter(Collider collider)
//        {
//            if (sourceTrigger == null) return;
//            if (bTriggered) return;
//            PlayerCollider playerCollider = collider.gameObject.GetComponent<PlayerCollider>();
//            if (playerCollider != null)
//            {
//                GameObject playerObject = playerCollider.playerObject;
//                if (playerObject != null)
//                    sourceTrigger.OnTrigger(playerObject);
//            }
//        }


//    }

//}

