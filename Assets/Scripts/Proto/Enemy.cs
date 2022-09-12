using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace proto
{
    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private int _health = 50;
        [SerializeField] private Player _player;
        [SerializeField] private float _distanceFromPlayer = 2.0f;
        [SerializeField] private Slider _healthBar;

        #endregion


        #region Hidden

        private Transform _transform;
        private NavMeshAgent _navMeshAgent;
        private Rigidbody _playerRigidBody;
        private bool _isInRange = false;

        #endregion


        #region Getter Setter

        public bool IsInRange
        {
            get { return _isInRange; }
            set { _isInRange = value; }
        }

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _playerRigidBody = _player.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            AgentMovesToDestination(_playerRigidBody.position);
        }

        #endregion


        #region Main

        private void AgentMovesToDestination(Vector3 destination)
        {
            float distance = Vector3.Distance(destination, transform.position);

            if (distance > _distanceFromPlayer)
            {
                if (_navMeshAgent.enabled == false)
                {
                    _navMeshAgent.enabled = true;
                }

                _navMeshAgent.SetDestination(destination);
            }
            else
            {
                if (_navMeshAgent.enabled == true)
                {
                    _navMeshAgent.enabled = false;
                }
            }

            _transform.LookAt(new Vector3(_playerRigidBody.position.x, _transform.position.y, _playerRigidBody.position.z));
        }

        public void TakeDamage()
        {
            if (_isInRange)
            {
                if (_health - 25 > 0)
                {
                    _health -= 25;
                    _healthBar.value = (float)_health / 50.0f;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        #endregion
    }
}
