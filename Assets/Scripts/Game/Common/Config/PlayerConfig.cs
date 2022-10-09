using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace Game
{
    [System.Serializable]
    public struct PlayerConfigData
    {
        public int nPrefabId;
        public int nHP;
        public int nAtk;
        public int nMoveSpeed;
    }
    public class PlayerConfig : MonoBehaviour
    {
        public PlayerConfigData data;
    }
}

