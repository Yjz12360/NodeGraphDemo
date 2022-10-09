using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerControl : MonoBehaviour
    {
        public float nMoveSpeed = 3.0f;
        public Animator modelAnimator;
        // Start is called before the first frame update
        private bool bAttacking = false;
        private float nAttackTime = 0.5f;
        private float nAttackTimer = 0;
        private GameObject attackObject;
        private bool bMoving = false;
        private CharacterController characterController;
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            Transform attackTrans = transform.Find("AttackCollider");
            if (attackTrans != null)
                attackObject = attackTrans.gameObject;
        }

        // Update is called once per frame
        void Update()
        {

            if (!bAttacking)
            {
                float nMoveDistance = nMoveSpeed * Time.deltaTime;
                Vector3 move = Vector3.zero;
                if (Input.GetKey(KeyCode.W))
                    move.z += nMoveDistance;
                if (Input.GetKey(KeyCode.S))
                    move.z -= nMoveDistance;
                if (Input.GetKey(KeyCode.A))
                    move.x -= nMoveDistance;
                if (Input.GetKey(KeyCode.D))
                    move.x += nMoveDistance;
                move = move.normalized * nMoveDistance;
                if (move != Vector3.zero)
                {
                    characterController.Move(move);
                    transform.forward = move.normalized;
                    if (!bMoving)
                    {
                        bMoving = true;
                        if (modelAnimator != null)
                            modelAnimator.SetTrigger("Move");
                    }
                }
                else
                {
                    if (bMoving)
                    {
                        bMoving = false;
                        if (modelAnimator != null)
                            modelAnimator.SetTrigger("EndMove");
                    }
                }
            }

            if (attackObject != null)
            {
                if (bAttacking)
                {
                    nAttackTimer -= Time.deltaTime;
                    if (nAttackTimer <= nAttackTime / 2)
                    {
                        attackObject.SetActive(true);
                    }
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
                    bAttacking = true;
                    if (modelAnimator != null)
                        modelAnimator.SetTrigger("Attack");
                }
            }

            if (!characterController.isGrounded)
            {
                Vector3 down = Vector3.down;
                down *= 9.8f * 5 * Time.deltaTime;
                characterController.Move(down);
            }

        }

    }
}

