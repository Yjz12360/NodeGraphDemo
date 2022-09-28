using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameObjectId : MonoBehaviour
    {
        public int nGameObjectId = -1;

        private void Start()
        {
            if(nGameObjectId == -1)
            {
                nGameObjectId = GameObjectManager.GenId();
            }
            GameObjectManager.Register(nGameObjectId, gameObject);
        }
    }

    public static class GameObjectManager
    {
        private static Dictionary<int, GameObject> tGameObjects = new Dictionary<int, GameObject>();
        private static int nCurrId = 0;

        public static void Register(int nId, GameObject gameObject)
        {
            if(gameObject == null)
            {
                Debug.LogError("Register GameObject error: gameObject is null.");
                return;
            }
            if(tGameObjects.ContainsKey(nId))
            {
                Debug.LogError("Register GameObject error: nId already exists.");
                return;
            }
            tGameObjects[nId] = gameObject;
        }

        public static int GenId()
        {
            do
            {
                nCurrId++;
            } while (tGameObjects.ContainsKey(nCurrId));
            return nCurrId;
        }

        public static GameObject GetObjectById(int nId)
        {
            if (!tGameObjects.ContainsKey(nId))
                return null;
            return tGameObjects[nId];
        }
    }
}


