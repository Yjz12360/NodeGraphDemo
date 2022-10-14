using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    [ExecuteInEditMode]
    class OrderChildConfigId : MonoBehaviour
    {
        private int GenTriggerId()
        {
            for (int i = 1; i < 10000; ++i)
            {
                if (transform.Find(i.ToString()) == null)
                    return i;
            }
            return -1;
        }

        private void Update()
        {
            for(int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                int triggerId;
                if (!int.TryParse(child.name, out triggerId))
                {
                    triggerId = GenTriggerId();
                    child.name = triggerId.ToString();
                }
            }
        }

    }
}
