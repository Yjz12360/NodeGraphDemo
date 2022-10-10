using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MonsterControl : MonoBehaviour
    {
        public Animator modelAnimator;

        private Vector3 movePos;
        private bool bMoving = false;
        private bool bDead = false;
        private float nDeadTime = 1.0f;
        private float nDeadTimer = 0;
        public void Move(Vector3 targetPos)
        {
            movePos = targetPos;
            bMoving = true;
            if (modelAnimator != null)
                modelAnimator.SetTrigger("Move");
        }

        public void Dead()
        {
            bDead = true;
            if (modelAnimator != null)
                modelAnimator.SetTrigger("Die");
        }

        private void Update()
        {
            if (bDead)
            {
                nDeadTimer += Time.deltaTime;
                if (nDeadTimer >= nDeadTime)
                    Destroy(gameObject);
                return;
            }
            if(bMoving)
            {
                CltObjectData cltObjectData = gameObject.GetComponent<CltObjectData>();
                if(cltObjectData != null)
                {
                    float nSpeed = cltObjectData.nSpeed;
                    Vector3 dir = (movePos - transform.position).normalized;
                    transform.position = transform.position + dir * nSpeed * Time.deltaTime;
                    transform.forward = dir;
                    float nDistance = Vector3.Magnitude(movePos - transform.position);
                    if (nDistance <= 0.2f)
                    {
                        bMoving = false;
                        if (modelAnimator != null)
                        {
                            //modelAnimator.

                            modelAnimator.SetTrigger("EndMove");

                        }
                    }
                        
                }
            }
        }
    }
}

