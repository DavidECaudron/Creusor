using UnityEngine;

namespace refacto_deux
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PlayerMovementScriptableObject _playerMovementScriptableObject;

        #endregion


        #region Updates

        private void Start()
        {
            _playerMovementScriptableObject.NextPosition = _playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.position;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        #endregion


        #region Main

        private void Movement()
        {
            Vector3 vector1 = new Vector3(_playerMovementScriptableObject.NextPosition.x,
                                          _playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y,
                                          _playerMovementScriptableObject.NextPosition.z);

            Vector3 vector2 = new Vector3(_playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.x,
                                          _playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y,
                                          _playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.z);

            if (!_playerMovementScriptableObject.IsUsingSpell)
            {
                _playerMovementScriptableObject.PlayerScriptableObject.PlayerTransform.LookAt(vector1);
            }
            else
            {
                _playerMovementScriptableObject.PlayerScriptableObject.PlayerTransform.LookAt(_playerMovementScriptableObject.LookPosition);
            }

            float distance = Vector3.Distance(vector1, vector2);

            if (distance < _playerMovementScriptableObject.MoveSpeed / 4)
            {
                return;
            }

            if (_playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.velocity.magnitude < _playerMovementScriptableObject.MoveSpeed / 2)
            {
                Vector3 direction = _playerMovementScriptableObject.NextPosition - vector2;

                _playerMovementScriptableObject.PlayerScriptableObject.PlayerRigidbody.AddForce(direction.normalized * _playerMovementScriptableObject.MoveSpeed * 2 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }

        #endregion
    }
}
