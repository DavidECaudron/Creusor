using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class ChestPosition : MonoBehaviour
    {
        #region Inspector

        [Header("Other")]
        public bool _hasBeenUsed = false;
        public Transform _transform;
        public int _index;

        [Header("Chest Pack")]
        public int _nbChest = 0;
        [Range(0, 20)] public float _spawnAreaRange;

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
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_transform.position, _spawnAreaRange);
        }

        #endregion
    }
}
