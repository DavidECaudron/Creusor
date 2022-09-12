using UnityEngine;

namespace Refacto_Trois
{
    [CreateAssetMenu(fileName = "GameManager_Data", menuName = "Data/GameManager_Data")]
    public class GameManager_Data : ScriptableObject
    {
        #region Inspector

        [Header("Script")]
        [SerializeField] private GameManager _gameManager;

        [Header("Data")]
        [SerializeField] private Player_Data _playerData;
        [SerializeField] private Enemy_Data _enemyData;
        [SerializeField] private Ability_Data _abilityData;

        [Header("Tweak")]
        [SerializeField] private int _timerInSeconds;

        #endregion


        #region Get Set

        public GameManager GameManager { get => _gameManager; private set => _gameManager = value; }
        public Player_Data PlayerData { get => _playerData; private set => _playerData = value; }
        public Enemy_Data EnemyData { get => _enemyData; private set => _enemyData = value; }
        public Ability_Data AbilityData { get => _abilityData; private set => _abilityData = value; }

        #endregion
    }
}
