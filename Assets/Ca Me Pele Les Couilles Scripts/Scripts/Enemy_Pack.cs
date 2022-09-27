using UnityEngine;

namespace caca
{
    public class Enemy_Pack : MonoBehaviour
    {
        #region Inspector

        [Header("Other")]
        public Game_Manager _gameManager;
        public GameObject _enemyPrefab;
        public Transform _playerTransform;
        public Transform _cameraTransform;

        [Header("Enemy Pack")]
        public EnemyType _enemyType;
        public int _numberOfEnemy;
        [Range(0, 5)] public int _spawnAreaRange;
        public float _movementSpeed;
        public float _maxHealth;
        public float _damage;
        public float _attackPerSecond;

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

                Vector3 randomPos = new (_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

                GameObject clone = Instantiate(_enemyPrefab, randomPos, _transform.rotation, _transform);
                Enemy enemyClone = clone.GetComponent<Enemy>();

                enemyClone._playerTransform = _playerTransform;
                enemyClone._cameraTransform = _cameraTransform;
                enemyClone._enemyType = _enemyType;
                enemyClone._movementSpeed = _movementSpeed;
                enemyClone._maxHealth = _maxHealth;
                enemyClone._damage = _damage;
                enemyClone._attackPerSecond = _attackPerSecond;

                _gameManager.AddEnemyInTable(enemyClone);

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

        public void HideEnemy()
        {
            for (int i = 0; i < _enemyTable.Length; i++)
            {
                _enemyTable[i].HideEnemy();
            }
        }

        public void ShowEnemy()
        {
            for (int i = 0; i < _enemyTable.Length; i++)
            {
                _enemyTable[i].ShowEnemy();
            }
        }

        #endregion
    }
}
