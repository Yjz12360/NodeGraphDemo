
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
        private Dictionary<int, SvrTriggerData> tTriggerConfig = new Dictionary<int, SvrTriggerData>();
        private SceneNodeGraph.SvrNodeGraphManager nodeGraphManager;

        public int AddPlayer(PlayerConfigData configData, Vector3 position)
        {
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.nGameObjectId = nCurrId;
            svrObjectData.nType = GameObjectType.Player;
            svrObjectData.position = position;
            svrObjectData.nMaxHP = configData.nHP;
            svrObjectData.nCurrHP = configData.nHP;
            svrObjectData.nAtk = configData.nAtk;
            svrObjectData.nSpeed = configData.nMoveSpeed;
            tObjectData[nCurrId] = svrObjectData;
            GameMessager.S2CAddPlayer(nCurrId, configData, position);
            return nCurrId++;
        }

        public int AddMonster(int nConfigId, Vector3 position) { return AddMonster(nConfigId, position, -1); }
        public int AddMonster(int nConfigId, Vector3 position, int nStaticId)
        {
            if (!Config.Monster.ContainsKey(nConfigId))
            {
                Debug.LogError("AddMonster error, config not found.");
                return -1;
            }
            MonsterConfigData configData = Config.Monster[nConfigId];
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.nGameObjectId = nCurrId;
            svrObjectData.nType = GameObjectType.Monster;
            svrObjectData.nStaticId = nStaticId;
            svrObjectData.nSpeed = configData.nMoveSpeed;
            svrObjectData.nMaxHP = configData.nHP;
            svrObjectData.nCurrHP = configData.nHP;
            svrObjectData.nAtk = configData.nAtk;
            svrObjectData.position = position;
            tObjectData[nCurrId] = svrObjectData;
            tMonsterAI[nCurrId] = new MonsterAI(svrObjectData);
            tMonsterAI[nCurrId].SetActive(configData.bActiveAI);
            nodeGraphManager.OnMonsterNumChange(GetMonsterNum());
            GameMessager.S2CAddMonster(nCurrId, configData, position);
            return nCurrId++;
        }

        public bool HasMonster()
        {
            foreach (var pair in tObjectData)
            {
                SvrObjectData svrObjectData = pair.Value;
                if (svrObjectData.nType == GameObjectType.Monster)
                    return true;
            }
            return false;
        }

        public int GetMonsterNum()
        {
            int nNum = 0;
            foreach (SvrObjectData svrObjectData in tObjectData.Values)
                if (svrObjectData.nType == GameObjectType.Monster)
                    nNum++;
            return nNum;
        }

        public int AddTrigger(Vector3 position, int nStaticId, SvrTriggerData triggerData)
        {
            SvrObjectData svrObjectData = new SvrObjectData();
            svrObjectData.nGameObjectId = nCurrId;
            svrObjectData.nType = GameObjectType.Trigger;
            svrObjectData.nStaticId = nStaticId;
            svrObjectData.position = position;
            tObjectData[nCurrId] = svrObjectData;
            tTriggerConfig[nCurrId] = triggerData;
            GameMessager.S2CAddTrigger(nCurrId, position);
            return nCurrId++;
        }

        public void RemoveObject(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId))
                return;
            if (tObjectData[nObjectId].nType == GameObjectType.Monster)
                if (tMonsterAI.ContainsKey(nObjectId))
                    tMonsterAI.Remove(nObjectId);
            if (tObjectData[nObjectId].nType == GameObjectType.Trigger)
                if (tTriggerConfig.ContainsKey(nObjectId))
                    tTriggerConfig.Remove(nObjectId);
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
            foreach (SvrObjectData svrObjectData in tObjectData.Values)
            {
                if (svrObjectData.nType == GameObjectType.Monster &&
                    svrObjectData.nStaticId == nStaticId)
                    return svrObjectData;
            }
            return null;
        }

        public void AttackHitMonster(int nPlayerId, int nMonsterId)
        {
            if (!tObjectData.ContainsKey(nPlayerId) || !tObjectData.ContainsKey(nMonsterId))
                return;
            SvrObjectData playerData = tObjectData[nPlayerId];
            SvrObjectData monsterData = tObjectData[nMonsterId];
            Vector3 playerPos = playerData.position;
            Vector3 monsterPos = monsterData.position;
            float nSqrDistance = Vector3.SqrMagnitude(playerPos - monsterPos);
            if (nSqrDistance > 100) return;
            monsterData.nCurrHP -= playerData.nAtk;
            if (monsterData.nCurrHP <= 0)
                MonsterDead(nMonsterId);
        }

        public void DoExplosion(Vector3 center, float nRadius, int nDamage)
        {
            List<int> affectedRoles = GetRangeRoles(center, nRadius);
            foreach (int nObjectId in affectedRoles)
                MonsterDoDamage(nObjectId, nDamage);
        }

        public void MonsterDoDamage(int nObjectId, int nDamage)
        {
            if (!tObjectData.ContainsKey(nObjectId))
                return;
            SvrObjectData monsterData = tObjectData[nObjectId];
            monsterData.nCurrHP -= nDamage;
            if (monsterData.nCurrHP <= 0)
            {
                MonsterDead(nObjectId);
            }
        }
        public void MonsterDead(int nObjectId)
        {
            if (!tObjectData.ContainsKey(nObjectId)) return;
            if (tObjectData[nObjectId].nType != GameObjectType.Monster) return;
            nodeGraphManager.OnMonsterDead(nObjectId);
            RemoveObject(nObjectId);
            nodeGraphManager.OnMonsterNumChange(GetMonsterNum());
            GameMessager.S2CMonsterDead(nObjectId);
        }

        public void OnSyncPlayerPos(int nObjectId, Vector3 position)
        {
            if (!tObjectData.ContainsKey(nObjectId)) return;
            if (tObjectData[nObjectId].nType != GameObjectType.Player) return;
            tObjectData[nObjectId].position = position;
        }

        public void OnActivateTrigger(int nPlayerId, int nObjectId)
        {
            if (!tObjectData.ContainsKey(nPlayerId)) return;
            if (!tTriggerConfig.ContainsKey(nObjectId)) return;
            SvrTriggerData triggerData = tTriggerConfig[nObjectId];
            if (string.IsNullOrEmpty(triggerData.sConfigFile)) return;
            SvrObjectData playerData = tObjectData[nPlayerId];
            Vector3 playerPos = playerData.position;
            Vector3 triggerPos = triggerData.position;
            float nSqrDistance = Vector3.SqrMagnitude(playerPos - triggerPos);
            if (nSqrDistance > 100) return;
            nodeGraphManager.OnTriggerNodeGraph(triggerData.sConfigFile);
            if (triggerData.bTriggerOnce)
                RemoveObject(nObjectId);
        }

        public List<int> GetRangeRoles(Vector3 center, float nRadius)
        {
            List<int> result = new List<int>();
            foreach (var pair in tObjectData)
            {
                SvrObjectData objectData = pair.Value;
                if(objectData.nType == GameObjectType.Monster || objectData.nType == GameObjectType.Player)
                {
                    Vector3 rolePos = objectData.position;
                    float nSqrDistance = Vector3.SqrMagnitude(rolePos - center);
                    if (nSqrDistance <= nRadius * nRadius)
                        result.Add(pair.Key);
                }
            }
            return result;
        }

        private void InitGameObjects()
        {
            Transform playerData = transform.Find("Player").GetChild(0);
            PlayerConfig playerConfig = playerData.gameObject.GetComponent<PlayerConfig>();
            if (playerConfig == null)
            {
                Debug.LogError("PlayerConfig not found");
                return;
            }
            AddPlayer(playerConfig.data, playerData.position);
            Transform staticMonster = transform.Find("StaticMonster");
            for(int i = 0; i < staticMonster.childCount; ++i)
            {
                Transform child = staticMonster.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                int nStaticId = int.Parse(child.name);
                ConfigId configId = child.gameObject.GetComponent<ConfigId>();
                if (configId == null || !Config.Monster.ContainsKey(configId.nConfigId)) continue;
                AddMonster(configId.nConfigId, child.position, nStaticId);
            }
            Transform triggerConfig = transform.Find("TriggerConfig");
            for(int i = 0; i < triggerConfig.childCount; ++i)
            {
                Transform child = triggerConfig.GetChild(i);
                if (!child.gameObject.activeSelf) continue;
                int nStaticId = int.Parse(child.name);
                TriggerConfig config = child.gameObject.GetComponent<TriggerConfig>();
                if (config != null)
                {
                    SvrTriggerData svrTriggerData = new SvrTriggerData();
                    svrTriggerData.position = child.position;
                    svrTriggerData.sConfigFile = $"{config.nodeGraph.name}.json";
                    svrTriggerData.bTriggerOnce = config.bTriggerOnce;
                    AddTrigger(child.position, nStaticId, svrTriggerData);
                }
            }
        }

        private void Start()
        {
            nodeGraphManager = gameObject.GetComponent<SceneNodeGraph.SvrNodeGraphManager>();
            InitGameObjects();
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
