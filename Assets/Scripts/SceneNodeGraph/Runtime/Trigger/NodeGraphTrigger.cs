using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace SceneNodeGraph
{
    public class NodeGraphTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        public Collider checkCollider;
        public TextAsset nodeConfigFile;

        public void Start()
        {
            if (nodeConfigFile == null) return;
            GameObject targetObject = checkCollider != null ? checkCollider.gameObject : gameObject;
            NodeGraphTriggerObj triggerObj = targetObject.AddComponent<NodeGraphTriggerObj>();
            triggerObj.sourceTrigger = this;
        }
        public void OnTrigger(Collider collider)
        {
            Messager.C2STriggerNodeGraph($"{nodeConfigFile.name}.json");
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

