using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CltGame: MonoBehaviour
    {
        private GameObject monsterPrefab;
        private GameObject triggerPrefab;
        private Transform monsterRoot;
        private Transform triggerRoot;

        private void Awake()
        {
            monsterPrefab = (GameObject)Resources.Load("GamePrefabs/Monster/Monster");
            triggerPrefab = (GameObject)Resources.Load("GamePrefabs/Trigger/Trigger");
            monsterRoot = GameObject.Find("GameObjects/Monsters").transform;
            triggerRoot = GameObject.Find("GameObjects/Triggers").transform;
        }

        private Dictionary<int, GameObject> tGameObjects = new Dictionary<int, GameObject>();

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

        //public bool HasMonster()
        //{
        //    foreach (var pair in tGameObjects)
        //    {
        //        GameObject gameObject = pair.Value;
        //        CltObjectData dataComp = gameObject.GetComponent<CltObjectData>();
        //        if (dataComp != null && dataComp.data.nType == GameObjectType.Monster)
        //            return true;
        //    }
        //    return false;
        //}

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

    }
}

