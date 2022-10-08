
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Game
{
    public enum ActionState
    {
        Running = 1,
        Finish = 2,
    }
    public class MonsterAction
    {
        public SvrObjectData objectData;
        public virtual ActionState Update(float nDeltaTime)
        {
            return ActionState.Finish;
        }
    }

    public class IdleAction : MonsterAction
    {
        private float nTime = 0.0f;
        private float nTimer = 0.0f;
        public IdleAction(SvrObjectData objectData, float nTime)
        {
            this.objectData = objectData;
            this.nTime = nTime;
        }

        public override ActionState Update(float nDeltaTime)
        {
            nTimer += nDeltaTime;
            if (nTimer >= nTime)
                return ActionState.Finish;
            else
                return ActionState.Running;
        }
    }

    public class MoveAction : MonsterAction
    {
        private Vector3 targetPos;
        public MoveAction(SvrObjectData objectData, Vector3 targetPos)
        {
            this.objectData = objectData;
            this.targetPos = targetPos;
        }

        public override ActionState Update(float nDeltaTime)
        {
            Vector3 dir = (targetPos - objectData.position).normalized;
            float nSpeed = objectData.nSpeed;
            Vector3 newPos = objectData.position + dir * nSpeed * nDeltaTime;
            objectData.position = newPos;
            float nDistance = (targetPos - newPos).magnitude;
            if (nDistance > 0.2f)
                return ActionState.Running;
            else
                return ActionState.Finish;
        }
    }

    public class MonsterAI
    {
        private SvrObjectData objectData;
        private MonsterAction currAction = null;

        public MonsterAI(SvrObjectData objectData)
        {
            this.objectData = objectData;
        }
        public void Update(float nDeltaTime)
        {
            if(currAction == null)
            {
                if(Random.Range(0.0f, 1.0f) < 0.3f)
                {
                    float nTime = Random.Range(0.6f, 2.0f);
                    currAction = new IdleAction(objectData, nTime);
                }
                else
                {
                    float nRad = Random.Range(0, 2 * Mathf.PI);
                    float nDistance = Random.Range(0.8f, 3.0f);
                    Vector3 currPos = objectData.position;
                    float nTargetX = currPos.x + Mathf.Cos(nRad) * nDistance;
                    float nTargetZ = currPos.z + Mathf.Sin(nRad) * nDistance;
                    Vector3 targetPos = new Vector3(nTargetX, currPos.y, nTargetZ);
                    currAction = new MoveAction(objectData, targetPos);
                    GameMessager.S2CMonsterMove(objectData.nGameObjectId, targetPos);
                }
            }

            if(currAction != null)
            {
                ActionState actionState = currAction.Update(nDeltaTime);
                if (actionState == ActionState.Finish)
                    currAction = null;
            }
        }
    }
}
