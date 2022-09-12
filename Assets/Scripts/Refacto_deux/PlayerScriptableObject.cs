using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        #region Hidden

        private Transform _playerTransform;
        private Rigidbody _playerRigidbody;

        #endregion


        #region Get Set

        public Transform PlayerTransform { get => _playerTransform; set => _playerTransform = value; }
        public Rigidbody PlayerRigidbody { get => _playerRigidbody; set => _playerRigidbody = value; }

        #endregion
    }
}
