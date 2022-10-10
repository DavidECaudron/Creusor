using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class DigAudio : MonoBehaviour
    {
        public AudioSource audioSource;

        public AudioClip[] digClips = new AudioClip[3];

        void Start()
        {
            audioSource.PlayOneShot(digClips[Random.Range(0, digClips.Length)], 0.3f);
        }
    }
}
