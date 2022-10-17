using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject itemPrefabs;
        public ParticleSystem itemParticleSystem ;
        private ParticleSystem.Particle[] itemParticles;
        
        public int spawnCount = 30; 
        List<Rigidbody> rbItems = new List<Rigidbody>();

        public AudioSource coinAudioSource;

        public void DropCoins()
        {
            StartCoroutine(NewCoinDrop());
        }

        IEnumerator NewCoinDrop()
        {
            itemParticleSystem.Play();
            coinAudioSource.Play();
            
            yield return new WaitForSeconds(0.85f);
            SpawnItems();

            yield return null;
        }

        void SpawnItems()
        {
            
            itemParticles = new ParticleSystem.Particle[spawnCount];
            itemParticleSystem.GetParticles(itemParticles);

            for(int i = 0; i < spawnCount; i++)
            {
                Vector3 thePosition = transform.TransformPoint(itemParticles[i].position);
                GameObject Go = Instantiate(itemPrefabs, thePosition, Quaternion.identity);
                //rbItems.Add(item.GetComponent<Rigidbody>());
                //rbItems[i].AddForce(RandomVector(1, -1f) * 100);
            }            
        }


        private Vector3 RandomVector(float min, float max)
        {
            var x = Random.Range(min, max);
            var y = Random.Range(0, 1.5f) * 2;
            var z = Random.Range(min, max);

            return new Vector3(x, y, z);

        }

            // Update is called once per frame
            void Update()
            {
            
            }
        }
}
