﻿
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
        public List<Transform> staticMonsters = new List<Transform>();
        public Transform playerBornPos;

        private int nCurrId = 1;
        private Dictionary<int, SvrObjectData> tObjectData = new Dictionary<int, SvrObjectData>();
        private Dictionary<int, MonsterAI> tMonsterAI = new Dictionary<int, MonsterAI>();
        private SceneNodeGraph.SvrNodeGraphManager nodeGraphManager;

        public int AddPlayer(Vector3 position)
        {
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.commonData.nGameObjectId = nCurrId;
            svrObjectData.commonData.nType = GameObjectType.Player;
            svrObjectData.commonData.nSpeed = 3.0f;
            svrObjectData.position = position;
            tObjectData[nCurrId] = svrObjectData;
            GameMessager.S2CAddPlayer(nCurrId, position);
            return nCurrId++;
        }

        public int AddMonster(Vector3 position) { return AddMonster(position, -1); }
        public int AddMonster(Vector3 position, int nStaticId)
        {
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.commonData.nGameObjectId = nCurrId;
            svrObjectData.commonData.nType = GameObjectType.Monster;
            svrObjectData.commonData.nStaticId = nStaticId;
            svrObjectData.commonData.nSpeed = 3.0f; // TODO
            svrObjectData.position = position;
            tObjectData[nCurrId] = svrObjectData;
            tMonsterAI[nCurrId] = new MonsterAI(svrObjectData);
            nodeGraphManager.OnMonsterNumChange(GetMonsterNum());
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

        public int GetMonsterNum()
        {
            return tObjectData.Count;
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

        public SvrObjectData GetMonsterByStaticId(int nStaticId)
        {
            foreach(SvrObjectData svrObjectData in tObjectData.Values)
            {
                if (svrObjectData.commonData.nType == GameObjectType.Monster &&
                    svrObjectData.commonData.nStaticId == nStaticId)
                    return svrObjectData;
            }
            return null;
        }

        public void AttackHitMonster(int nObjectId)
        {
            MonsterDead(nObjectId); // TODO
        }

        public void MonsterDead(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId)) return;
            if (tObjectData[nObjectId].commonData.nType != GameObjectType.Monster) return;
            nodeGraphManager.OnMonsterDead(nObjectId);
            nodeGraphManager.OnMonsterNumChange(GetMonsterNum());
            RemoveObject(nObjectId);
            GameMessager.S2CMonsterDead(nObjectId);
        }

        public void OnSyncPlayerPos(int nObjectId, float nPosX, float nPosY, float nPosZ)
        {
            if (!tObjectData.ContainsKey(nObjectId)) return;
            if (tObjectData[nObjectId].commonData.nType != GameObjectType.Player) return;
            tObjectData[nObjectId].position.x = nPosX;
            tObjectData[nObjectId].position.y = nPosY;
            tObjectData[nObjectId].position.z = nPosZ;
        }

        private void Start()
        {
            nodeGraphManager = gameObject.GetComponent<SceneNodeGraph.SvrNodeGraphManager>();
            for(int i = 0; i < staticMonsters.Count; ++i)
            {
                Transform transform = staticMonsters[i];
                if (transform == null) continue;
                int nStaticId = int.Parse(transform.name);
                //int nStaticId = 0;
                //if (!int.TryParse(transform.name, out nStaticId))
                //    nStaticId = i + 1;
                AddMonster(transform.position, nStaticId);
            }
            if(playerBornPos != null)
            {
                AddPlayer(playerBornPos.position);
            }
            else
            {
                Debug.LogError("SvrGame Init Player error: playerBornPos is null.");
            }
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
