using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace refacto_deux
{
    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [SerializeField] public int _maxHealth = 50;
        [SerializeField] public GameObject _playerGameObject;
        [SerializeField] private float _distanceFromPlayer = 2.0f;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private float _attackPerSecond;
        [SerializeField] private Transform _position;

        #endregion


        #region Hidden

        private Transform _transform;
        private NavMeshAgent _navMeshAgent;
        private Rigidbody _playerRigidBody;
        private int _currentHealth;
        private bool _isPlayerInRange;
        private Transform _camera;
        private Transform _canvas;
        private PlayerInput _playerInput;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _playerRigidBody = _playerGameObject.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
            _isPlayerInRange = false;
            _camera = Camera.main.transform;
            _canvas = _healthBar.transform.parent;
            _playerInput = _playerGameObject.GetComponent<PlayerInput>();
        }

        private void Update()
        {
            AgentMovesToDestination(_playerRigidBody.position);
            _canvas.LookAt(new Vector3(transform.position.x, _camera.position.y, _camera.position.z));
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
                    _isPlayerInRange = false;
                    _playerInput._isEnemyInRange = false;
                    _navMeshAgent.enabled = true;
                }

                _navMeshAgent.SetDestination(destination);
            }
            else
            {
                if (_navMeshAgent.enabled == true)
                {
                    _isPlayerInRange = true;
                    _playerInput._isEnemyInRange = true;
                    StartCoroutine(AttackCoroutine());
                    _navMeshAgent.enabled = false;
                }
            }

            _transform.LookAt(new Vector3(_playerRigidBody.position.x, _transform.position.y, _playerRigidBody.position.z));
        }

        public void TakeDamage(int damage)
        {
            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
                _healthBar.value = (float)_currentHealth / _maxHealth;
            }
            else
            {
                _isPlayerInRange = false;
                _playerInput._isEnemyInRange = false;
                Destroy(gameObject);
            }
        }

        #endregion


        #region Util

        IEnumerator AttackCoroutine()
        {
            while (_isPlayerInRange == true)
            {
                yield return new WaitForSeconds(1.0f / _attackPerSecond);

                Collider[] hitColliders = Physics.OverlapSphere(_position.position, 0.5f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.transform.CompareTag("player"))
                    {
                        hitCollider.transform.parent.parent.GetComponent<Player>().TakeDamage(10);
                    }
                }
            }
        }

        #endregion
    }
}
