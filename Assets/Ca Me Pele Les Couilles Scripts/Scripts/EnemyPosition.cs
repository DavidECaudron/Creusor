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
        public int _nbEnemy = 0;
        [Range(0, 10)] public int _spawnAreaRange;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
        }

        #endregion
    }
}
