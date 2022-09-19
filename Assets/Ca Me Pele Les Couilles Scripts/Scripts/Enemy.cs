using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace caca
{
    public enum EnemyType
    {
        Melee,
        Ranged,
        Dasher
    }

    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Transform _playerTransform;
        public Transform _cameraTransform;
        public Transform _attackDetection;
        public Transform _transform;
        public NavMeshAgent _navMeshAgent;
        public Slider _healthSlider;
        public GameObject _graphics;
        public GameObject _body;
        public GameObject _meleeModel;
        public GameObject _rangedModel;
        public Vector3 _initialPosition;
        public bool _isPlayerInDetectionRange = false;

        [Header("Enemy")]
        public EnemyType _enemyType;
        public float _movementSpeed;
        public float _maxHealth;
        public float _currentHealth;
        public float _damage;
        public float _attackPerSecond;

        public bool _isAlive = true;

        #endregion


        #region Hidden

        private Player _player;
        private Transform _healthSliderTransform;
        private MeshRenderer _meshRenderer;

        private bool _isPlayerInAttackRange = false;
        private bool _isAttacking = false;
        private int _heightIndex = 0;

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
            _player = _playerTransform.GetComponent<Player>();

            _navMeshAgent.speed = _movementSpeed;
            _navMeshAgent.acceleration = _movementSpeed;
            _currentHealth = _maxHealth;
            _initialPosition = _transform.position;
            _meshRenderer = _body.GetComponentInChildren<MeshRenderer>();

            if (_enemyType == EnemyType.Melee)
            {
                _meleeModel.SetActive(true);
            }

            if (_enemyType == EnemyType.Ranged)
            {
                _rangedModel.SetActive(true);
            }
        }

        private void LateUpdate()
        {
            _healthSliderTransform.LookAt(new Vector3(_transform.position.x, _cameraTransform.position.y, _cameraTransform.position.z));

            Movement();
        }

        #endregion


        #region Main

        public void Movement()
        {
            if (_isAlive == true)
            {
                Vector3 graphicsPosition = _graphics.transform.position;

                if (_heightIndex > 0)
                {
                    _graphics.transform.position = new Vector3(graphicsPosition.x, 0.0f, graphicsPosition.z);
                }
                else
                {
                    _graphics.transform.position = new Vector3(graphicsPosition.x, 1.0f, graphicsPosition.z);
                }

                if (_isPlayerInDetectionRange == true)
                {
                    if (_enemyType == EnemyType.Melee)
                    {
                        if (Vector3.Distance(_playerTransform.position, _transform.position) > 2.0f)
                        {
                            _isPlayerInAttackRange = false;
                            _isAttacking = false;
                            _navMeshAgent.SetDestination(_playerTransform.position);
                        }
                        else
                        {
                            _transform.LookAt(_playerTransform);

                            _navMeshAgent.SetDestination(_transform.position);
                            _isPlayerInAttackRange = true;

                            if (_isAttacking == false)
                            {
                                Collider[] hitColliders = Physics.OverlapSphere(_attackDetection.position, 0.5f);

                                foreach (var hitCollider in hitColliders)
                                {
                                    if (hitCollider.transform.CompareTag("player"))
                                    {
                                        StartCoroutine(AttackCoroutine());
                                    }
                                }

                                _isAttacking = true;
                            }
                        }
                    }

                    if (_enemyType == EnemyType.Ranged)
                    {
                        if (Vector3.Distance(_playerTransform.position, _transform.position) > 6.0f)
                        {
                            _isPlayerInAttackRange = false;
                            _isAttacking = false;
                            _navMeshAgent.SetDestination(_playerTransform.position);
                        }
                        else
                        {
                            _transform.LookAt(_playerTransform);

                            _navMeshAgent.SetDestination(_transform.position);
                            _isPlayerInAttackRange = true;

                            //if (_isAttacking == false)
                            //{
                            //    Collider[] hitColliders = Physics.OverlapSphere(_attackDetection.position, 0.5f);

                            //    foreach (var hitCollider in hitColliders)
                            //    {
                            //        if (hitCollider.transform.CompareTag("player"))
                            //        {
                            //            StartCoroutine(AttackCoroutine());
                            //        }
                            //    }

                            //    _isAttacking = true;
                            //}
                        }
                    }
                }
                else
                {
                    if (Vector3.Distance(_initialPosition, _transform.position) > 0.5f)
                    {
                        _isPlayerInAttackRange = false;
                        _isAttacking = false;
                        _navMeshAgent.SetDestination(_initialPosition);
                    }
                }
            }
        }

        public void TakeDamage(float damage)
        {
            if (_currentHealth - damage > 0)
            {
                StartCoroutine(DamageFeedback());
                _currentHealth -= damage;
                _healthSlider.value = (_currentHealth/_maxHealth);
            }
            else
            {
                _isAlive = false;
                _navMeshAgent.enabled = false;
                _transform.position = new Vector3(-100.0f,-5.0f,-100.0f);
            }
        }

        #endregion


        #region Utils

        IEnumerator AttackCoroutine()
        {
            while (_isAlive == true && _isPlayerInAttackRange == true)
            {
                _player.TakeDamage(_damage);

                yield return new WaitForSecondsRealtime(1 / _attackPerSecond);
            }
        }

        IEnumerator DamageFeedback()
        {
            _meshRenderer.material.SetFloat("_DamageColorAmount", 1.0f);

            yield return new WaitForSecondsRealtime(0.10f);

            _meshRenderer.material.SetFloat("_DamageColorAmount", 0.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("digMask"))
            {
                _heightIndex += 1;
            }

            if (other.CompareTag("shockwaveMask"))
            {
                _heightIndex += 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("digMask"))
            {
                _heightIndex -= 1;
            }

            if (other.CompareTag("shockwaveMask"))
            {
                _heightIndex -= 1;
            }
        }

        #endregion
    }
}
