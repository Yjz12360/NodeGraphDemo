
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
        public SvrObjectData controlObject;
        public SvrGame game;

        public virtual void Start()
        {

        }

        public virtual ActionState Update(float nDeltaTime)
        {
            return ActionState.Finish;
        }
    }

    public class IdleAction : MonsterAction
    {
        private float nTime = 0.0f;
        private float nTimer = 0.0f;
        public IdleAction(float nTime)
        {
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
        public MoveAction(Vector3 targetPos)
        {
            this.targetPos = targetPos;
        }

        public override void Start()
        {
            GameMessager.S2CMonsterMove(controlObject.nGameObjectId, targetPos);
        }

        public override ActionState Update(float nDeltaTime)
        {
            Vector3 dir = (targetPos - controlObject.position).normalized;
            float nSpeed = controlObject.nSpeed;
            Vector3 newPos = controlObject.position + dir * nSpeed * nDeltaTime;
            controlObject.position = newPos;
            float nDistance = (targetPos - newPos).magnitude;
            if (nDistance > 0.2f)
                return ActionState.Running;
            else
                return ActionState.Finish;
        }
    }

    public class ChaseAction : MonsterAction
    {
        private int nTargetId;
        private float nChaseTime = 0.0f;
        private float nTimer = 0.0f;
        private float nStopDistance = 1.0f;
        public ChaseAction(int nTargetId, float nTime, float nStopDistance)
        {
            this.nTargetId = nTargetId;
            this.nChaseTime = nTime;
            this.nStopDistance = nStopDistance;
        }

        public override void Start()
        {
            GameMessager.S2CMonsterChase(controlObject.nGameObjectId, nTargetId, nChaseTime, nStopDistance);
        }

        public override ActionState Update(float nDeltaTime)
        {
            nTimer += nDeltaTime;
            if (nTimer >= nChaseTime)
                return ActionState.Finish;

            SvrObjectData targetObject = game.GetObject(nTargetId);
            Vector3 dir = (targetObject.position - controlObject.position).normalized;
            float nSpeed = controlObject.nSpeed;
            Vector3 newPos = controlObject.position + dir * nSpeed * nDeltaTime;
            controlObject.position = newPos;
            float nSqrDistance = (targetObject.position - newPos).sqrMagnitude;
            if (nSqrDistance <= nStopDistance * nStopDistance)
                return ActionState.Finish;

            return ActionState.Running;
        }
    }
}
