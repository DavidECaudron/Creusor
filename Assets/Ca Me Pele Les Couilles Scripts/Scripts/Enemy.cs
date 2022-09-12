using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace caca
{
    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Transform _playerTransform;
        public Transform _cameraTransform;
        public Slider _healthSlider;
        public Vector3 _initialPosition;
        public bool _isPlayerInRange = false;

        [Header("Enemy")]
        [Range(0, 10)] public int _movementSpeed;
        public int _maxHealth;

        #endregion


        #region Hidden

        private Transform _transform;
        private Transform _healthSliderTransform;
        private NavMeshAgent _navMeshAgent;

        private int _currentHealth;
        
        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _healthSliderTransform = _healthSlider.gameObject.transform.parent.parent.GetComponent<Transform>(); 
        }

        private void Start()
        {
            _navMeshAgent.speed = _movementSpeed;
            _navMeshAgent.acceleration = _movementSpeed;
            _currentHealth = _maxHealth;
            _initialPosition = _transform.position;
        }

        private void LateUpdate()
        {
            _healthSliderTransform.LookAt(new Vector3(_transform.position.x, _cameraTransform.position.y, _cameraTransform.position.z));

            if (_isPlayerInRange == true)
            {
                if (Vector3.Distance(_playerTransform.position, _transform.position) > 2.0f)
                {
                    _navMeshAgent.SetDestination(_playerTransform.position);
                }
                else
                {
                    _navMeshAgent.SetDestination(_transform.position);
                }
            }
            else
            {
                if (Vector3.Distance(_initialPosition, _transform.position) > 0.5f)
                {
                    _navMeshAgent.SetDestination(_initialPosition);
                }
            }
        }

        #endregion


        #region Main

        public void TakeDamage(int damage)
        {
            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
                _healthSlider.value = ((float)_currentHealth/(float)_maxHealth);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
