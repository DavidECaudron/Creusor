using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class Coconut : MonoBehaviour
    {
        public AudioSource _coconutAudioSource;
        public AudioClip _coconutFallClip;


        private void OnCollisionEnter(Collision col)
        {
            if(col.gameObject.tag == "ground")
            {
                _coconutAudioSource.PlayOneShot(_coconutFallClip, 0.2f);
            }
        }
    }
}
