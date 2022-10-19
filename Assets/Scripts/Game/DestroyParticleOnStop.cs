using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DestroyParticleOnStop : MonoBehaviour
    {
        private float nDuration;

        private void Start()
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps == null) return;
            nDuration = ps.main.duration;
            StartCoroutine("WaitDestroy");
        }

        IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(nDuration);
            Destroy(gameObject);
        }

    }
}

