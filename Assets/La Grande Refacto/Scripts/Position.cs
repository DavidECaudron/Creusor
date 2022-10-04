using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class Position : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private Player _playerScript;
        [SerializeField] private EnemyPack _enemyPackScript;
        [SerializeField] private Transform _transform;
        [SerializeField] private SphereCollider _sphereCollider;

        [Header("Tweak")]
        [SerializeField, Range(1, 10)] private float _radius;

        #endregion


        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                List<Enemy> temp = _enemyPackScript.GetEnemyList();

                foreach (Enemy item in temp)
                {
                    item.SetIsPlayerInRange(true);
                    StartCoroutine(item.ChasePlayer(_playerScript));
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("player"))
            {
                List<Enemy> temp = _enemyPackScript.GetEnemyList();

                foreach (Enemy item in temp)
                {
                    item.SetIsPlayerInRange(false);
                    StartCoroutine(item.MoveToInitialPosition());
                }
            }
        }

        #endregion


        #region Gizmos

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_transform.position, _radius);
        }

        #endregion


        #region Getter

        public Transform GetTransform()
        {
            return _transform;
        }

        public SphereCollider GetSphereCollider()
        {
            return _sphereCollider;
        }

        public float GetRadius()
        {
            return _radius;
        }

        #endregion
    }
}
