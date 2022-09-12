using UnityEngine;

namespace Refacto_Trois
{
    [CreateAssetMenu(fileName = "Player_Data", menuName = "Data/Player_Data")]
    public class Player_Data : ScriptableObject
    {
        #region Inspector

        [Header("Script")]
        [SerializeField] private Player _player;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerCamera _playerCamera;

        [Header("Data")]
        [SerializeField] private GameManager_Data _gameManagerData;

        [Header("Tweak")]
        [SerializeField] private int _movementSpeed;
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _damage;

        #endregion


        #region Get Set

        public Player Player { get => _player; set => _player = value; }
        public PlayerInput PlayerInput { get => _playerInput; set => _playerInput = value; }
        public PlayerMovement PlayerMovement { get => _playerMovement; set => _playerMovement = value; }
        public PlayerCamera PlayerCamera { get => _playerCamera; set => _playerCamera = value; }
        public GameManager_Data GameManagerData { get => _gameManagerData; private set => _gameManagerData = value; }
        public int MovementSpeed { get => _movementSpeed; private set => _movementSpeed = value; }
        public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
        public int Damage { get => _damage; private set => _damage = value; }

        #endregion
    }
}
