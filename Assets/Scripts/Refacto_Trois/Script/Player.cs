using UnityEngine;

namespace Refacto_Trois
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Player_Data _playerData;

        #endregion


        #region Hidden

        private Player _player;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerCamera _playerCamera;

        #endregion


        #region Updates

        private void Awake()
        {
            _player = gameObject.GetComponent<Player>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _playerMovement = gameObject.GetComponent<PlayerMovement>();
            _playerCamera = gameObject.GetComponent<PlayerCamera>();
        }

        private void Start()
        {
            _playerData.Player = _player;
            _playerData.PlayerInput = _playerInput;
            _playerData.PlayerMovement = _playerMovement;
            _playerData.PlayerCamera = _playerCamera;
        }

        #endregion
    }
}
