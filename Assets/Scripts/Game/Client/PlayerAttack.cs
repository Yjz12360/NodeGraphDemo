using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAttack : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            MonsterCollider monsterCollider = other.GetComponent<MonsterCollider>();
            if (monsterCollider != null)
            {
                GameObject monsterObject = monsterCollider.monsterObject;
                if(monsterObject != null)
                {
                    CltObjectData cltObjectData = monsterObject.GetComponent<CltObjectData>();
                    if (cltObjectData != null)
                    {
                        GameMessager.C2SAttackHitMonster(cltObjectData.commonData.nGameObjectId);
                    }
                }
            }
        }
    }
}

