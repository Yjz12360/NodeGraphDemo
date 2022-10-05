using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject followObject;

        public float nDistance = 8.0f;
        public float nHeight = 9.0f;
        public float nAngle = 50.0f;

        private void Update()
        {
            if (followObject == null) return;
            Vector3 followPos = followObject.transform.position;
            transform.position = new Vector3(followPos.x, followPos.y + nHeight, followPos.z - nDistance);
            transform.rotation = Quaternion.Euler(nAngle, 0, 0);
        }
    }
}

