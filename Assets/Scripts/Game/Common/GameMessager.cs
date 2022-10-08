using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class GameMessager
    {
        private static SvrGame SvrComp = null;
        private static SvrGame ServerComp
        {
            get
            {
                if (SvrComp == null)
                    SvrComp = GameObject.Find("NetWork/Server").GetComponent<SvrGame>();
                return SvrComp;
            }
        }

        private static CltGame CltComp = null;
        private static CltGame ClientComp
        {
            get
            {
                if (CltComp == null)
                    CltComp = GameObject.Find("NetWork/Client").GetComponent<CltGame>();
                return CltComp;
            }
        }

        public static void S2CAddPlayer(int nObjectId, Vector3 position)
        {
            ClientComp.AddPlayer(nObjectId, position);
        }

        public static void S2CAddMonster(int nObjectId, Vector3 position)
        {
            ClientComp.AddMonster(nObjectId, position);
        }

        public static void S2CMonsterMove(int nObjectId, Vector3 position)
        {
            ClientComp.MonsterMove(nObjectId, position);
        }
        public static void S2CMonsterDead(int nObjectId)
        {
            ClientComp.MonsterDead(nObjectId);
        }

        public static void C2SAttackHitMonster(int nObjectId)
        {
            ServerComp.AttackHitMonster(nObjectId);
        }

        public static void C2SSyncPlayerPos(int nObjectId, float nPosX, float nPosY, float nPosZ)
        {
            ServerComp.OnSyncPlayerPos(nObjectId, nPosX, nPosY, nPosZ);
        }
    }
}

