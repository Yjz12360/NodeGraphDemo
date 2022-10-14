using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    class TriggerConfigContainer : MonoBehaviour
    {
        private void Start()
        {
            GameObject gameTriggerContainer = GameObject.Find("GameObjects/Triggers");

            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                child.SetParent(gameTriggerContainer.transform, true);
            }
        }

    }
}
