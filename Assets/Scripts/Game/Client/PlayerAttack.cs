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
                if(monsterObject != null && playerObject != null)
                {
                    CltObjectData playerData = playerObject.GetComponent<CltObjectData>();
                    CltObjectData monsterData = monsterObject.GetComponent<CltObjectData>();
                    if (monsterData != null && playerData != null)
                    {
                        int nPlayerId = playerData.commonData.nGameObjectId;
                        int nMonsterId = monsterData.commonData.nGameObjectId;
                        GameMessager.C2SAttackHitMonster(nPlayerId, nMonsterId);
                    }
                }
            }
        }
    }
}

