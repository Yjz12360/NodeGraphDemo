
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;

namespace Game
{
    class MonsterAI : MonoBehaviour
    {
        public float nSpeed = 10.0f;

        private int nCurrDir = 0;
        private float nCurrMoveTime = 0.0f;
        private float nCurrMoveTimer = 0.0f;

        private void SetMove()
        {
            nCurrDir = Random.Range(0, 4);
            nCurrMoveTime = Random.Range(0.5f, 2.0f);
            nCurrMoveTimer = 0.0f;
        }

        private void Start()
        {
            SetMove();
        }

        private void Update()
        {
            Vector3 translate = Vector3.zero;
            if (nCurrDir == 0)
                translate.z = translate.z + nSpeed * Time.deltaTime;
            if (nCurrDir == 1)
                translate.z = translate.z - nSpeed * Time.deltaTime;
            if (nCurrDir == 2)
                translate.x = translate.x - nSpeed * Time.deltaTime;
            if (nCurrDir == 3)
                translate.x = translate.x + nSpeed * Time.deltaTime;
            transform.Translate(translate);

            nCurrMoveTimer += Time.deltaTime;
            if (nCurrMoveTimer >= nCurrMoveTime)
                SetMove();
        }

    }
}
