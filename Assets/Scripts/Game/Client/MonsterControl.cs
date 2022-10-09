using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MonsterControl : MonoBehaviour
    {
        private Vector3 movePos;
        private bool bMoving = false;

        public void Move(Vector3 targetPos)
        {
            movePos = targetPos;
            bMoving = true;
        }

        private void Update()
        {
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
                        bMoving = false;
                }
            }
        }
    }
}

