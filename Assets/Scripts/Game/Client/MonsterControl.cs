using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MonsterControl : MonoBehaviour
    {
        public Animator modelAnimator;

        private float nMoveSpeed = 3.0f;

        private Vector3 movePos;
        private bool bMoving = false;

        private bool bDead = false;
        private float nDeadTime = 1.0f;
        private float nDeadTimer = 0;

        private bool bChasing = false;
        private GameObject chaseTarget;
        private float nChaseTime;
        private float nChaseTimer;
        private float nChaseStopDistance;
        public void Move(Vector3 targetPos)
        {
            movePos = targetPos;
            bMoving = true;
            modelAnimator?.SetTrigger("Move");
        }

        public void Chase(GameObject targetObject, float nChaseTime, float nStopDistance)
        {
            if (targetObject == null) return;
            bChasing = true;
            chaseTarget = targetObject;
            modelAnimator?.SetTrigger("Move");
            this.nChaseTime = nChaseTime;
            this.nChaseStopDistance = nStopDistance;
        }

        public void Dead()
        {
            bDead = true;
            modelAnimator?.SetTrigger("Die");
        }

        private void Start()
        {
            nMoveSpeed = 3;
            //CltObjectData cltObjectData = gameObject.GetComponent<CltObjectData>();
            //if (cltObjectData != null)
            //{
            //    nMoveSpeed = cltObjectData.nSpeed;
            //}
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
                Vector3 dir = (movePos - transform.position).normalized;
                transform.position = transform.position + dir * nMoveSpeed * Time.deltaTime;
                transform.forward = dir;
                float nDistance = Vector3.Magnitude(movePos - transform.position);
                if (nDistance <= 0.2f)
                {
                    bMoving = false;
                    modelAnimator?.SetTrigger("EndMove");
                }
            }
            if (bChasing && chaseTarget != null)
            {
                Vector3 targetPos = chaseTarget.transform.position;
                Vector3 dir = (targetPos - transform.position).normalized;
                transform.position = transform.position + dir * nMoveSpeed * Time.deltaTime;
                transform.forward = dir;
                nChaseTime += Time.deltaTime;
                float nDistance = Vector3.Magnitude(movePos - transform.position);
                if (nDistance <= nChaseStopDistance || nChaseTime >= nChaseTimer)
                {
                    bChasing = false;
                    modelAnimator?.SetTrigger("EndMove");
                }
            }
        }
    }
}

