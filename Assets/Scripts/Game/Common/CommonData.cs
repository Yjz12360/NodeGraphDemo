using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum GameObjectType
    {
        Player = 1,
        Monster = 2,
        Trigger = 3,
    }

    public class CommonData
    {
        public int nGameObjectId;
        public GameObjectType nType;
        public int nStaticId;
        public float nSpeed;
    }
}

