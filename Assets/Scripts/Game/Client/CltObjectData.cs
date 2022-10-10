using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CltObjectData : MonoBehaviour
    {
        public int nGameObjectId;
        public GameObjectType nType;
        public float nSpeed;
        public int nStaticId;
        public int nPrefabId;
        public int nMaxHP;
        public int nCurrHP;
        public int nAtk;
        public void Start()
        {
            this.hideFlags = HideFlags.NotEditable;
        }
    }
}

