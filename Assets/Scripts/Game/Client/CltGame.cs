using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CltGame: MonoBehaviour
    {
        private GameObject triggerPrefab;
        private Transform playerRoot;
        private Transform monsterRoot;
        private Transform triggerRoot;
        private Transform explosionRoot;

        private GameObject mainCamera;

        private GameObject localPlayer;
        private float nSyncTime = 0.2f;
        private float nSyncTimer = 0;

        private RolePrefabs rolePrefabs;
        private EffectPrefabs effectPrefabs;
        private void Awake()
        {
            triggerPrefab = (GameObject)Resources.Load("GamePrefabs/Trigger/Trigger");
            playerRoot = GameObject.Find("GameObjects/Players").transform;
            monsterRoot = GameObject.Find("GameObjects/Monsters").transform;
            triggerRoot = GameObject.Find("GameObjects/Triggers").transform;
            explosionRoot = GameObject.Find("Explosions").transform;

            mainCamera = GameObject.Find("Main Camera");

            GameObject gamePrefabs = GameObject.Find("GamePrefabs");
            rolePrefabs = gamePrefabs.GetComponent<RolePrefabs>();
            effectPrefabs = gamePrefabs.GetComponent<EffectPrefabs>();
        }

        private GameObject GetByTid(List<GameObject> prefabs, int nTid)
        {
            if (nTid >= 0 && nTid < prefabs.Count)
                return prefabs[nTid];
            return null;
        }

        private Dictionary<int, GameObject> tGameObjects = new Dictionary<int, GameObject>();

        public void AddPlayer(int nObjectId, PlayerConfigData configData, Vector3 position)
        {
            GameObject playerPrefab = GetByTid(rolePrefabs.prefabs, configData.nPrefabId);
            if (playerPrefab == null)
            {
                Debug.LogError($"CltGame AddPlayer error: prefab not exist. nPrefabId:{configData.nPrefabId}");
                return;
            }
            GameObject instance = Instantiate(playerPrefab);
            instance.transform.SetParent(playerRoot);
            instance.transform.position = position;
            instance.name = $"Player_{nObjectId}";
            CltObjectData cltObjectData = instance.GetComponent<CltObjectData>();
            if (cltObjectData == null)
                cltObjectData = instance.AddComponent<CltObjectData>();
            cltObjectData.nGameObjectId = nObjectId;
            cltObjectData.nType = GameObjectType.Player;
            cltObjectData.nMaxHP = configData.nHP;
            cltObjectData.nCurrHP = configData.nHP;
            cltObjectData.nAtk = configData.nAtk;
            cltObjectData.nSpeed = configData.nMoveSpeed;
            tGameObjects[nObjectId] = instance;
            localPlayer = instance;
            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                cameraFollow.followObject = localPlayer;
            }
        }

        public void AddMonster(int nObjectId, MonsterConfigData configData, Vector3 position, int nStaticId)
        {
            GameObject monsterPrefab = GetByTid(rolePrefabs.prefabs, configData.nPrefabId);
            if(monsterPrefab == null)
            {
                Debug.LogError($"CltGame AddMonster error: prefab not exist. nPrefabId:{configData.nPrefabId}");
                return;
            }
            GameObject instance = GameObject.Instantiate(monsterPrefab);
            instance.transform.SetParent(monsterRoot);
            instance.transform.position = position;
            instance.name = $"Monster_{nObjectId}";
            CltObjectData cltObjectData = instance.GetComponent<CltObjectData>();
            if (cltObjectData == null)
                cltObjectData = instance.AddComponent<CltObjectData>();
            cltObjectData.nGameObjectId = nObjectId;
            cltObjectData.nType = GameObjectType.Monster;
            cltObjectData.nSpeed = 3.0f;
            cltObjectData.nPrefabId = configData.nPrefabId;
            cltObjectData.nMaxHP = configData.nHP;
            cltObjectData.nCurrHP = configData.nHP;
            cltObjectData.nAtk = configData.nAtk;
            cltObjectData.nStaticId = nStaticId;
            tGameObjects[nObjectId] = instance;
        }

        public void MonsterMove(int nObjectId, Vector3 position)
        {
            GameObject monsterObject = GetObject(nObjectId);
            if (monsterObject == null) return;
            MonsterControl monsterControl = monsterObject.GetComponent<MonsterControl>();
            if (monsterControl == null) return;
            monsterControl.Move(position);
        }

        public void RemoveObject(int nObjectId) { RemoveObject(nObjectId, true); }
        public void RemoveObject(int nObjectId, bool bDestroy)
        {
            if (!tGameObjects.ContainsKey(nObjectId))
                return;
            if (bDestroy)
                Destroy(tGameObjects[nObjectId]);
            tGameObjects.Remove(nObjectId);
        }

        public GameObject GetObject(int nObjectId)
        {
            if (!tGameObjects.ContainsKey(nObjectId)) return null;
            return tGameObjects[nObjectId];
        }

        public GameObject GetMonsterByStaticId(int nStaticId)
        {
            foreach (GameObject gameObject in tGameObjects.Values)
            {
                CltObjectData cltObjectData = gameObject.GetComponent<CltObjectData>();
                if (cltObjectData.nStaticId == nStaticId)
                    return gameObject;
            }
            return null;
        }

        public void MonsterDead(int nObjectId)
        {
            if (!tGameObjects.ContainsKey(nObjectId))
                return;
            MonsterControl monsterControl = tGameObjects[nObjectId].GetComponent<MonsterControl>();
            monsterControl.Dead();
            RemoveObject(nObjectId, false);
        }

        public void AddTrigger(int nObjectId, Vector3 position)
        {
            GameObject instance = GameObject.Instantiate(triggerPrefab);
            instance.transform.SetParent(triggerRoot);
            instance.transform.position = position;
            instance.name = $"Trigger_{nObjectId}";
            CltObjectData gameObjectData = instance.GetComponent<CltObjectData>();
            if (gameObjectData == null)
                gameObjectData = instance.AddComponent<CltObjectData>();
            gameObjectData.nGameObjectId = nObjectId;
            gameObjectData.nType = GameObjectType.Trigger;
            tGameObjects[nObjectId] = instance;
        }

        public void PlayExplosion(Vector3 position, int nEffectId, float nDisplayScale)
        {
            GameObject explosionPrefab = GetByTid(effectPrefabs.prefabs, nEffectId);
            GameObject instance = Instantiate(explosionPrefab);
            instance.transform.SetParent(explosionRoot);
            instance.transform.position = position;
            instance.transform.localScale = new Vector3(nDisplayScale, nDisplayScale, nDisplayScale);
            ParticleSystem ps = instance.GetComponent<ParticleSystem>();
            if(ps == null)
            {
                Debug.LogError("Explosion has no ParticleSystem component.");
                return;
            }
            instance.AddComponent<DestroyParticleOnStop>();
        }

        private void Update()
        {
            if(localPlayer != null)
            {
                nSyncTimer += Time.deltaTime;
                if (nSyncTimer >= nSyncTime)
                {
                    nSyncTimer = 0;
                    CltObjectData objectData = localPlayer.GetComponent<CltObjectData>();
                    if(objectData != null)
                    {
                        int nObjectId = objectData.nGameObjectId;
                        Vector3 pos = localPlayer.transform.position;
                        GameMessager.C2SSyncPlayerPos(nObjectId, pos);
                    }
                }
            }
        }

    }
}

