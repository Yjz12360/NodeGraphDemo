using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [ExecuteAlways]
    public class StaticMonsterId : MonoBehaviour
    {
        private static Dictionary<int, GameObject> instances = new Dictionary<int, GameObject>();
        private static int GenId()
        {
            for(int i = 1; i < 10000; ++i)
            {
                if (!instances.ContainsKey(i))
                    return i;
            }
            return -1;
        }

        public int nId;

        private void Start()
        {
            this.hideFlags = HideFlags.NotEditable;
            nId = GenId();
            instances[nId] = gameObject;
        }

        private void OnDestroy()
        {
            if(instances.ContainsKey(nId))
            {
                instances.Remove(nId);
            }
        }
    }
}

