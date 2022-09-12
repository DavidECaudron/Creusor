using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "PlayerInputScriptableObject", menuName = "ScriptableObjects/PlayerInputScriptableObject")]
    public class PlayerInputScriptableObject : ScriptableObject
    {
        #region Inspector

        [SerializeField] private PlayerScriptableObject _playerScriptableObject;
        [SerializeField] private PlayerMovementScriptableObject _playerMovementScriptableObject;
        [SerializeField] private PlayerAbilitiesScriptableObject _playerAbilitiesScriptableObject;
        [SerializeField] private DigThrowDirtScriptableObject _digThrowDirtScriptableObject;
        [SerializeField] private DigShockwaveScriptableObject _digShockwaveScriptableObject;

        #endregion


        #region Hidden

        private Camera _camera;

        private Ray _ray;
        private RaycastHit _raycasthit;
        private Vector2 _mousePosition;

        private bool _isLeftClicking;

        #endregion


        #region Get Set

        public PlayerScriptableObject PlayerScriptableObject { get => _playerScriptableObject; set => _playerScriptableObject = value; }
        public PlayerMovementScriptableObject PlayerMovementScriptableObject { get => _playerMovementScriptableObject; set => _playerMovementScriptableObject = value; }
        public PlayerAbilitiesScriptableObject PlayerAbilitiesScriptableObject { get => _playerAbilitiesScriptableObject; set => _playerAbilitiesScriptableObject = value; }
        public DigThrowDirtScriptableObject DigThrowDirtScriptableObject { get => _digThrowDirtScriptableObject; set => _digThrowDirtScriptableObject = value; }
        public DigShockwaveScriptableObject DigShockwaveScriptableObject { get => _digShockwaveScriptableObject; set => _digShockwaveScriptableObject = value; }
        public Camera Camera { get => _camera; set => _camera = value; }

        public Ray Ray { get => _ray; set => _ray = value; }
        public RaycastHit RaycastHit { get => _raycasthit; set => _raycasthit = value; }
        public Vector2 MousePosition { get => _mousePosition; set => _mousePosition = value; }

        public bool IsLeftClicking { get => _isLeftClicking; set => _isLeftClicking = value; }

        #endregion
    }
}
