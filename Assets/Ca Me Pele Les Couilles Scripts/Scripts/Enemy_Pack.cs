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
        public int _numberOfEnemyMelee;
        public int _numberOfEnemyRanged;
        [Range(0, 20)] public float _spawnAreaRange;
        public float _movementSpeed;
        public float _maxHealth;
        public float _damageMelee;
        public float _damageRanged;
        public float _attackPerSecondMelee;
        public float _attackPerSecondRanged;

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

            _sphereCollider.radius = _spawnAreaRange * 3.0f;

            SpawnEnemies();
        }

        #endregion


        #region Main

        public void SpawnEnemies()
        {
            int temp = _numberOfEnemyMelee + _numberOfEnemyRanged;

            _enemyTable = new Enemy[temp];

            _enemyType = EnemyType.Melee;

            for (int i = 0; i < _numberOfEnemyMelee; i += 1)
            {
                float randomX = Random.Range(-_spawnAreaRange * 0.6f, _spawnAreaRange * 0.6f);
                float randomZ = Random.Range(-_spawnAreaRange * 0.6f, _spawnAreaRange * 0.6f);

                Vector3 randomPos = new (_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

                GameObject clone = Instantiate(_enemyPrefab, randomPos, _transform.rotation, _transform);
                Enemy enemyClone = clone.GetComponent<Enemy>();

                enemyClone._playerTransform = _playerTransform;
                enemyClone._cameraTransform = _cameraTransform;
                enemyClone._enemyType = _enemyType;
                enemyClone._movementSpeed = _movementSpeed;
                enemyClone._maxHealth = _maxHealth;
                enemyClone._damageMelee = _damageMelee;
                enemyClone._attackPerSecondMelee = _attackPerSecondMelee;

                _gameManager.AddEnemyInTable(enemyClone);

                _enemyTable[i] = enemyClone;
            }

            _enemyType = EnemyType.Ranged;

            for (int i = 0; i < _numberOfEnemyRanged; i += 1)
            {
                float randomX = Random.Range(-_spawnAreaRange * 0.6f, _spawnAreaRange * 0.6f);
                float randomZ = Random.Range(-_spawnAreaRange * 0.6f, _spawnAreaRange * 0.6f);

                Vector3 randomPos = new(_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

                GameObject clone = Instantiate(_enemyPrefab, randomPos, _transform.rotation, _transform);
                Enemy enemyClone = clone.GetComponent<Enemy>();

                enemyClone._playerTransform = _playerTransform;
                enemyClone._cameraTransform = _cameraTransform;
                enemyClone._enemyType = _enemyType;
                enemyClone._movementSpeed = _movementSpeed;
                enemyClone._maxHealth = _maxHealth;
                enemyClone._damageRanged = _damageRanged;
                enemyClone._attackPerSecondRanged = _attackPerSecondRanged;

                _gameManager.AddEnemyInTable(enemyClone);

                int tempI = i + _numberOfEnemyMelee;

                _enemyTable[tempI] = enemyClone;
            }
        }

        public void ChasePlayer()
        {
            _sphereCollider.enabled = false;

            for (int i = 0; i < _enemyTable.Length; i++)
            {
                _enemyTable[i]._isPlayerInDetectionRange = true;
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
