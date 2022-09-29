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

        #endregion


        #region Hidden

        private Transform _transform;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();

            Destroy(gameObject, _timeBeforeDestroy);
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
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
