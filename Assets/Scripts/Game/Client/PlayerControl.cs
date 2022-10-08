using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerControl : MonoBehaviour
    {
        public float nMoveSpeed = 3.0f;
        // Start is called before the first frame update
        private bool bAttacking = false;
        private float nAttackTime = 0.3f;
        private float nAttackTimer = 0;
        private GameObject attackObject;
        void Start()
        {
            Transform attackTrans = transform.Find("AttackSign");
            if (attackTrans != null)
                attackObject = attackTrans.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            float nMoveDistance = nMoveSpeed * Time.deltaTime;
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                move = new Vector3(0, 0, nMoveDistance);
            if (Input.GetKey(KeyCode.S))
                move = new Vector3(0, 0, -nMoveDistance);
            if (Input.GetKey(KeyCode.A))
                move = new Vector3(-nMoveDistance, 0, 0);
            if (Input.GetKey(KeyCode.D))
                move = new Vector3(nMoveDistance, 0, 0);
            if(move != Vector3.zero)
            {
                transform.position = transform.position + move;
                transform.forward = move.normalized;
            }

            if (attackObject != null)
            {
                if (bAttacking)
                {
                    nAttackTimer -= Time.deltaTime;
                    if (nAttackTimer <= 0)
                    {
                        nAttackTimer = 0;
                        attackObject.SetActive(false);
                        bAttacking = false;
                    }
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    nAttackTimer = nAttackTime;
                    attackObject.SetActive(true);
                    bAttacking = true;
                }
            }

        }

    }
}

