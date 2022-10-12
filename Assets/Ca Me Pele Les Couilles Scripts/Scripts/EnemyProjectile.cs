using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class EnemyProjectile : MonoBehaviour
    {
        #region Inspector

        public Vector3 _nextPosition;
        public float _damage;
        public float _movementSpeed;
        public float _timeBeforeDestroy;

        public AudioSource audioSource;

        public AudioClip[] projectileShootClips = new AudioClip[3];
        public AudioClip[] projectileHitClips = new AudioClip[3];

        private GameObject graphicsGroup;
        #endregion


        #region Hidden

        private Transform _transform;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();

            Destroy(gameObject, _timeBeforeDestroy);
            graphicsGroup = transform.GetChild(0).gameObject;
        }

        private void Start()
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            StartCoroutine(InitializeProjectile());         
        }

        private void LateUpdate()
        {
            Movement();
        }

        #endregion


        #region Main

        public void Movement()
        {
            _transform.Translate(_movementSpeed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
        }

        #endregion


        #region Utils

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                other.transform.parent.parent.GetComponent<Player>().TakeDamage(_damage);
                audioSource.PlayOneShot(projectileShootClips[Random.Range(0, projectileShootClips.Length)], 0.3f);
                StartCoroutine(DestroyProjectile());
            }
        }

        #endregion

        IEnumerator InitializeProjectile()
        {
            float elapsedTime = 0;
            float duration = 0.4f;
            audioSource.PlayOneShot(projectileShootClips[Random.Range(0, projectileShootClips.Length)], 0.3f);

            while(elapsedTime < duration)
            {
                Vector3 lerpScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), Vector3.one, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                transform.localScale = lerpScale;
                yield return null;
            }

            transform.localScale = Vector3.one;
            yield return null;
        }
        IEnumerator DestroyProjectile()
        {
            graphicsGroup.SetActive(false);
            audioSource.PlayOneShot(projectileHitClips[Random.Range(0, projectileHitClips.Length)], 0.3f);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
            yield return null;
        }

    }
}
