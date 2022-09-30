using UnityEngine;

namespace caca
{
    public class Chest_Pack : MonoBehaviour
    {
        #region Inspector

        [Header("Other")]
        public Game_Manager _gameManager;
        public GameObject _chestPrefab;
        public Transform _areaTransform;

        [Header("Enemy Pack")]
        public int _numberOfChest;
        [Range(0, 10)] public int _spawnAreaRange;

        #endregion


        #region Hidden

        private Transform _transform;
        private SphereCollider _sphereCollider;
        private Chest[] _chestTable;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _sphereCollider = GetComponent<SphereCollider>();

            _sphereCollider.radius = _spawnAreaRange;

            float tempSize = (_sphereCollider.radius / 3) + 0.5f;

            _areaTransform.localScale = new Vector3(tempSize, _areaTransform.localScale.y, tempSize);

            SpawnEnemies();
        }

        #endregion


        #region Main

        public void SpawnEnemies()
        {
            _chestTable = new Chest[_numberOfChest];

            for (int i = 0; i < _numberOfChest; i += 1)
            {
                int randomX = Random.Range(-_spawnAreaRange, _spawnAreaRange + 1);
                int randomZ = Random.Range(-_spawnAreaRange, _spawnAreaRange + 1);

                Vector3 randomPos = new (_transform.position.x + randomX, _transform.position.y, _transform.position.z + randomZ);

                GameObject clone = Instantiate(_chestPrefab, randomPos, _chestPrefab.transform.rotation, _transform);
                Chest chestClone = clone.GetComponent<Chest>();

                chestClone.transform.localScale *= 0.75f;
                chestClone._areaMask = _areaTransform.gameObject;

                _gameManager.AddChestInTable(chestClone);

                _chestTable[i] = chestClone;
            }
        }

        #endregion


        #region Util

        public void TrapChest(int indexChestPack, int indexEnemyPack)
        {
            for (int i = 0; i < _chestTable.Length; i++)
            {
                _chestTable[i]._isTrapped = true;
                _chestTable[i]._indexChestPack = indexChestPack;
                _chestTable[i]._indexEnemyPack = indexEnemyPack;
            }
        }

        public void UnTrapChest()
        {
            for (int i = 0; i < _chestTable.Length; i++)
            {
                _chestTable[i]._isTrapped = false;
            }
        }

        #endregion
    }
}
