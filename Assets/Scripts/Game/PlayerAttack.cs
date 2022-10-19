using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAttack : MonoBehaviour
    {
        public GameObject playerObject;
        private void OnTriggerEnter(Collider other)
        {
            MonsterCollider monsterCollider = other.GetComponent<MonsterCollider>();
            if (monsterCollider != null)
            {
                GameObject monsterObject = monsterCollider.monsterObject;
                if (monsterObject != null && playerObject != null)
                {
                    GameObjectId playerId = playerObject.GetComponent<GameObjectId>();
                    GameObjectId monsterId = monsterObject.GetComponent<GameObjectId>();
                    if(playerId != null && monsterId != null)
                    {
                        LuaFuncsCaller.DoCall("OnAttackHit", playerId.ID, monsterId.ID);
                    }
                }
            }
        }
    }
}

