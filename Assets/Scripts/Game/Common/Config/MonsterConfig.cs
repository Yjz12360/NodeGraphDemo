﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace Game
{
    [System.Serializable]
    public struct MonsterConfigData
    {
        public int nPrefabId;
        public int nHP;
        public int nAtk;
        public int nMoveSpeed;
        public bool bActiveAI;
    }
    public class MonsterConfig : MonoBehaviour
    {
        public MonsterConfigData data;
    }
}
