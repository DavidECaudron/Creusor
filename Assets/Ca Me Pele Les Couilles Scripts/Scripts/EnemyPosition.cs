using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class EnemyPosition : MonoBehaviour
    {
        #region Inspector

        [Header("Other")]
        public bool _hasBeenUsed = false;
        public Transform _transform;

        [Header("Enemy Pack")]
        public int _nbEnemyMelee = 0;
        public int _nbEnemyRanged = 0;
        [Range(1, 10)] public int _spawnAreaRange;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
        }

        #endregion


        #region Gizmos

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_transform.position, _spawnAreaRange);
        }

        #endregion
    }
}
