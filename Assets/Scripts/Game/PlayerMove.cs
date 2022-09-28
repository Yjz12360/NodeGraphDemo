using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerMove : MonoBehaviour
    {
        public float nSpeed = 10.0f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 translate = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                translate.z = translate.z + nSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                translate.z = translate.z - nSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                translate.x = translate.x - nSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                translate.x = translate.x + nSpeed * Time.deltaTime;
            transform.Translate(translate);

        }
    }
}

