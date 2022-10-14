using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    [ExecuteInEditMode]
    class EditorTriggerId : MonoBehaviour
    {
        private int GenTriggerId()
        {
            Transform container = transform.parent;
            for (int i = 1; i < 10000; ++i)
            {
                if (container.Find(i.ToString()) == null)
                    return i;
            }
            return -1;
        }

        public int triggerId;

        void Start()
        {
            if (!int.TryParse(gameObject.name, out triggerId))
            {
                triggerId = GenTriggerId();
                gameObject.name = triggerId.ToString();
            }
            hideFlags = HideFlags.NotEditable;
        }
    }
}
