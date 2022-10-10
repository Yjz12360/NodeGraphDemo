
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Game
{
    public class MonsterAI
    {
        private SvrObjectData controlObject;
        private SvrGame game;
        private MonsterAction currAction = null;
        private bool bActive = true;

        public MonsterAI(SvrObjectData objectData, SvrGame game)
        {
            this.controlObject = objectData;
            this.game = game;
        }

        public void SetActive(bool bActive)
        {
            this.bActive = bActive;
        }

        public void Update(float nDeltaTime)
        {
            if (!bActive) return;
            if(currAction == null)
            {
                SvrObjectData player = game.GetPlayer();
                if(player != null && Vector3.Distance(controlObject.position, player.position) < 8.0f)
                {
                    float nChaseTime = Random.Range(1.2f, 4.0f);
                    AddAction(new ChaseAction(player.nGameObjectId, nChaseTime, 10.0f));
                }
                else if(Random.Range(0.0f, 1.0f) < 0.3f)
                {
                    float nTime = Random.Range(0.6f, 2.0f);
                    AddAction(new IdleAction(nTime));
                }
                else
                {
                    float nRad = Random.Range(0, 2 * Mathf.PI);
                    float nDistance = Random.Range(0.8f, 3.0f);
                    Vector3 currPos = controlObject.position;
                    float nTargetX = currPos.x + Mathf.Cos(nRad) * nDistance;
                    float nTargetZ = currPos.z + Mathf.Sin(nRad) * nDistance;
                    Vector3 targetPos = new Vector3(nTargetX, currPos.y, nTargetZ);
                    AddAction(new MoveAction(targetPos));
                }
            }

            if(currAction != null)
            {
                ActionState actionState = currAction.Update(nDeltaTime);
                if (actionState == ActionState.Finish)
                    currAction = null;
            }
        }

        protected void AddAction(MonsterAction action)
        {
            if (currAction != null)
            {
                Debug.LogError("AI AddAction error: currAction exists.");
                return;
            }
            action.controlObject = controlObject;
            action.game = game;
            currAction = action;
            action.Start();
        }
    }
}
