
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Game
{
    public class SvrGame : MonoBehaviour
    {
        private int nCurrId = 1;
        private Dictionary<int, SvrObjectData> tObjectData = new Dictionary<int, SvrObjectData>();
        private Dictionary<int, MonsterAI> tMonsterAI = new Dictionary<int, MonsterAI>();

        public int AddMonster(Vector3 position)
        {
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.commonData.nGameObjectId = nCurrId;
            svrObjectData.commonData.nType = GameObjectType.Monster;
            svrObjectData.commonData.nSpeed = 3.0f; // TODO
            svrObjectData.position = position;
            tObjectData[nCurrId] = svrObjectData;
            tMonsterAI[nCurrId] = new MonsterAI(svrObjectData);
            GameMessager.S2CAddMonster(nCurrId, position);
            return nCurrId++;
        }

        public bool HasMonster()
        {
            foreach (var pair in tObjectData)
            {
                SvrObjectData svrObjectData = pair.Value;
                if (svrObjectData.commonData.nType == GameObjectType.Monster)
                    return true;
            }
            return false;
        }

        public void RemoveObject(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId))
                return;
            if (tObjectData[nObjectId].commonData.nType == GameObjectType.Monster)
                if (tMonsterAI.ContainsKey(nObjectId))
                    tMonsterAI.Remove(nObjectId);
            tObjectData.Remove(nObjectId);
        }

        public SvrObjectData GetObject(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId))
                return null;
            return tObjectData[nObjectId];
        }

        public void AttackHitMonster(int nObjectId)
        {
            MonsterDead(nObjectId); // TODO
        }

        public void MonsterDead(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId)) return;
            if (tObjectData[nObjectId].commonData.nType != GameObjectType.Monster) return;
            RemoveObject(nObjectId);
            GameMessager.S2CMonsterDead(nObjectId);
        }

        private void Update()
        {
            foreach(var pair in tMonsterAI)
            {
                MonsterAI monsterAI = pair.Value;
                monsterAI.Update(Time.deltaTime);
            }
        }
    }
}
