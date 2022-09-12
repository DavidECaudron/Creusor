using UnityEngine;

namespace refacto
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private float _moveSpeed;

        #endregion

        #region Hidden

        private Player _player;

        private Vector3 _nextPosition;
        private Vector3 _lookPosition;

        private bool _isUsingSpell;

        #endregion

        #region Get Set

        public Player Player { get => _player; private set => _player = value; }

        public Vector3 NextPosition { get => _nextPosition; set => _nextPosition = value; }
        public Vector3 LookPosition { get => _lookPosition; set => _lookPosition = value; }

        public bool IsUsingSpell { get => _isUsingSpell; set => _isUsingSpell = value; }

        #endregion

        #region Updates

        private void Awake()
        {
            Player = gameObject.GetComponent<Player>();
        }

        private void Start()
        {
            NextPosition = Player.Rigidbody.position;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        #endregion

        #region Main

        private void Movement()
        {
            Vector3 vector1 = new Vector3(NextPosition.x, Player.Rigidbody.position.y, NextPosition.z);
            Vector3 vector2 = new Vector3(Player.Rigidbody.position.x, Player.Rigidbody.position.y, Player.Rigidbody.position.z);

            if (!IsUsingSpell)
            {
                Player.Transform.LookAt(vector1);
            }
            else
            {
                Player.Transform.LookAt(LookPosition);
            }

            float distance = Vector3.Distance(vector1, vector2);

            if (distance < 2.0f) return;

            if (Player.Rigidbody.velocity.magnitude < _moveSpeed / 2)
            {
                Vector3 direction = NextPosition - vector2;

                Player.Rigidbody.AddForce(direction.normalized * _moveSpeed * 2 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }

        #endregion

        #region Utils
        #endregion
    }
}
