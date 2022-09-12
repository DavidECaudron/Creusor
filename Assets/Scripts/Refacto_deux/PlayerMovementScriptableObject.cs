using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "PlayerMovementScriptableObject", menuName = "ScriptableObjects/PlayerMovementScriptableObject")]
    public class PlayerMovementScriptableObject : ScriptableObject
    {
        #region Inspector

        [SerializeField] private PlayerScriptableObject _playerScriptableObject;

        [SerializeField] private float _moveSpeed;

        #endregion
        

        #region Hidden

        private Vector3 _nextPosition;
        private Vector3 _lookPosition;

        private bool _isUsingSpell;

        #endregion


        #region Get Set

        public PlayerScriptableObject PlayerScriptableObject { get => _playerScriptableObject; set => _playerScriptableObject = value; }

        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

        public Vector3 NextPosition { get => _nextPosition; set => _nextPosition = value; }
        public Vector3 LookPosition { get => _lookPosition; set => _lookPosition = value; }

        public bool IsUsingSpell { get => _isUsingSpell; set => _isUsingSpell = value; }

        #endregion
    }
}
