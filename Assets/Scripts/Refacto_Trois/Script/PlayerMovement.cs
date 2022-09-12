using UnityEngine;

namespace Refacto_Trois
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Player_Data _playerData;

        #endregion


        #region Hidden

        private Transform _playerTransform;

        private Vector3 _nextPosition;

        #endregion


        #region Updates

        private void Awake()
        {
            _playerTransform = gameObject.GetComponent<Transform>();
        }

        private void Start()
        {
            _nextPosition = _playerTransform.position;
        }

        private void Update()
        {
            Movement();
        }

        #endregion


        #region Main

        public void SetNextPosition(Vector3 nextPosition)
        {
            _nextPosition.x = nextPosition.x;
            _nextPosition.z = nextPosition.z;
        }

        public void Movement()
        {
            Vector3 direction = _playerTransform.position - _nextPosition;

            if (Vector3.Distance(_playerTransform.position, _nextPosition) >= 0.01f)
            {
                //_playerTransform.position += -direction.normalized * _playerData.MovementSpeed * Time.deltaTime;
                _playerTransform.Translate(-direction.normalized * _playerData.MovementSpeed * Time.deltaTime);
            }
        }

        #endregion
    }
}
