using UnityEngine;

namespace Refacto_Trois
{
    [CreateAssetMenu(fileName = "Enemy_Data", menuName = "Data/Enemy_Data")]
    public class Enemy_Data : ScriptableObject
    {
        #region Inspector

        [Header("Script")]
        [SerializeField] private Enemy _enemy;
        [SerializeField] private EnemyAttack _enemyAttack;
        [SerializeField] private EnemyMovement _enemyMovemnet;

        [Header("Data")]
        [SerializeField] private GameManager_Data _gameManagerData;

        [Header("Tweak")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _damage;

        #endregion


        #region Get Set

        public Enemy Enemy { get => _enemy; private set => _enemy = value; }
        public EnemyAttack EnemyAttack { get => _enemyAttack; private set => _enemyAttack = value; }
        public EnemyMovement EnemyMovement { get => _enemyMovemnet; private set => _enemyMovemnet = value; }
        public GameManager_Data GameManagerData { get => _gameManagerData; private set => _gameManagerData = value; }

        #endregion
    }
}
