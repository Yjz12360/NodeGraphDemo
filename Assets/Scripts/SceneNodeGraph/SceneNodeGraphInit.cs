using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneNodeGraph
{
    public class SceneNodeGraphInit : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            RegRuntimeNodeTypes.Initialize();
        }
    }
}

