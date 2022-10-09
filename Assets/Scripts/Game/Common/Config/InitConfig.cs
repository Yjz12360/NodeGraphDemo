using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

namespace Game
{

    public class InitConfig : MonoBehaviour
    {
        private void Awake()
        {
            Transform monster = transform.Find("Monster");
            for(int i = 0; i < monster.childCount; ++i)
            {
                Transform child = monster.GetChild(i);
                MonsterConfig monsterConfig = child.gameObject.GetComponent<MonsterConfig>();
                if (monsterConfig != null)
                {
                    int nMonsterTid = int.Parse(child.name);
                    Config.Monster[nMonsterTid] = monsterConfig.data;
                }
            }
        }
    }
}

