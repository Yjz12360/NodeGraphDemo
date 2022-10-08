using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CltGame: MonoBehaviour
    {
        private GameObject playerPrefab;
        private GameObject monsterPrefab;
        private GameObject triggerPrefab;
        private Transform playerRoot;
        private Transform monsterRoot;
        private Transform triggerRoot;

        private GameObject mainCamera;

        private GameObject localPlayer;
        private float nSyncTime = 0.2f;
        private float nSyncTimer = 0;

        private void Awake()
        {
            playerPrefab = (GameObject)Resources.Load("GamePrefabs/Player/Player");
            monsterPrefab = (GameObject)Resources.Load("GamePrefabs/Monster/Monster");
            triggerPrefab = (GameObject)Resources.Load("GamePrefabs/Trigger/Trigger");
            playerRoot = GameObject.Find("GameObjects/Players").transform;
            monsterRoot = GameObject.Find("GameObjects/Monsters").transform;
            triggerRoot = GameObject.Find("GameObjects/Triggers").transform;

            mainCamera = GameObject.Find("Main Camera");
        }

        private Dictionary<int, GameObject> tGameObjects = new Dictionary<int, GameObject>();

        public void AddPlayer(int nObjectId, Vector3 position)
        {
            GameObject instance = GameObject.Instantiate(playerPrefab);
            instance.transform.SetParent(playerRoot);
            instance.transform.position = position;
            instance.name = $"Player_{nObjectId}";
            CltObjectData gameObjectData = instance.GetComponent<CltObjectData>();
            if (gameObjectData == null)
                gameObjectData = instance.AddComponent<CltObjectData>();
            gameObjectData.commonData.nGameObjectId = nObjectId;
            gameObjectData.commonData.nType = GameObjectType.Player;
            gameObjectData.commonData.nSpeed = 3.0f;
            tGameObjects[nObjectId] = instance;
            localPlayer = instance;
            if (mainCamera != null)
            {
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                cameraFollow.followObject = localPlayer;
            }
        }

        public void AddMonster(int nObjectId, Vector3 position)
        {
            GameObject instance = GameObject.Instantiate(monsterPrefab);
            instance.transform.SetParent(monsterRoot);
            instance.transform.position = position;
            instance.name = $"Monster_{nObjectId}";
            CltObjectData gameObjectData = instance.GetComponent<CltObjectData>();
            if (gameObjectData == null)
                gameObjectData = instance.AddComponent<CltObjectData>();
            gameObjectData.commonData.nGameObjectId = nObjectId;
            gameObjectData.commonData.nType = GameObjectType.Monster;
            gameObjectData.commonData.nSpeed = 3.0f;
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

        public void RemoveObject(int nObjectId)
        {
            if (!tGameObjects.ContainsKey(nObjectId))
                return;
            Destroy(tGameObjects[nObjectId]);
            tGameObjects.Remove(nObjectId);
        }

        public GameObject GetObject(int nObjectId)
        {
            if (!tGameObjects.ContainsKey(nObjectId)) return null;
            return tGameObjects[nObjectId];
        }

        public void MonsterDead(int nObjectId)
        {
            RemoveObject(nObjectId);
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
            gameObjectData.commonData.nGameObjectId = nObjectId;
            gameObjectData.commonData.nType = GameObjectType.Trigger;
            tGameObjects[nObjectId] = instance;
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
                        int nObjectId = objectData.commonData.nGameObjectId;
                        Vector3 pos = localPlayer.transform.position;
                        GameMessager.C2SSyncPlayerPos(nObjectId, pos);
                    }
                }
            }
        }

    }
}

