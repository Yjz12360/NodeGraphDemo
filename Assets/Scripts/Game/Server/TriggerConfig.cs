using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace Game
{
    public struct SvrTriggerData
    {
        public Vector3 position;
        public string sConfigFile;
        public bool bTriggerOnce;
    }

    public class TriggerConfig : MonoBehaviour
    {
        public TextAsset nodeGraph;
        public bool bTriggerOnce = false;
    }

}

