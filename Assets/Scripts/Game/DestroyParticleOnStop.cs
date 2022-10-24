using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DestroyParticleOnStop : MonoBehaviour
    {
        private float duration;

        private void Start()
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps == null) return;
            duration = ps.main.duration;
            StartCoroutine("WaitDestroy");
        }

        IEnumerator WaitDestroy()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }

    }
}

