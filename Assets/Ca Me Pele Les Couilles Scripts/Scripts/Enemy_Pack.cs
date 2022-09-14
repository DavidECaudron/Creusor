using UnityEngine;

namespace caca
{
    public class Enemy_Pack : MonoBehaviour
    {
        #region Inspector

        [Header("Other")]
        public GameObject _enemyPrefab;
        public Transform _playerTransform;
        public Transform _cameraTransform;

        [Header("Enemy Pack")]
        [Range(0, 10)] public int _numberOfEnemy;
        [Range(0, 5)] public int _spawnAreaRange;

        #endregion


        #region Hidden

        private Transform _transform;
        private SphereCollider _sphereCollider;
        private Enemy[] _enemyTable;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            _sphereCollider.radius = _spawnAreaRange * 3;

            SpawnEnemies();
        }

        #endregion


        #region Main

        public void SpawnEnemies()
        {
            _enemyTable = new Enemy[_numberOfEnemy];

            for (int i = 0; i < _numberOfEnemy; i += 1)
            {
                int randomX = Random.Range(-_spawnAreaRange, _spawnAreaRange + 1);
                int randomZ = Random.Range(-_spawnAreaRange, _spawnAreaRange + 1);

                Vector3 randomPos = new Vector3(_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

                GameObject clone = Instantiate(_enemyPrefab, randomPos, _transform.rotation, _transform);
                Enemy enemyClone = clone.GetComponent<Enemy>();

                enemyClone._playerTransform = _playerTransform;
                enemyClone._cameraTransform = _cameraTransform;

                _enemyTable[i] = enemyClone;
            }
        }

        #endregion


        #region Utils

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                for (int i = 0; i < _enemyTable.Length; i++)
                {
                    _enemyTable[i]._isPlayerInDetectionRange = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("player"))
            {
                for (int i = 0; i < _enemyTable.Length; i++)
                {
                    _enemyTable[i]._isPlayerInDetectionRange = false;
                }
            }
        }

        #endregion
    }
}
