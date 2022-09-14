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
        public MeshRenderer _meshRenderer;
        public Slider _healthSlider;
        public GameObject _meleeModel;
        public GameObject _rangedModel;
        public Vector3 _initialPosition;
        public bool _isPlayerInDetectionRange = false;

        [Header("Enemy")]
        public EnemyType _enemyType;
        public int _movementSpeed;
        public int _maxHealth;
        public float _attackPerSecond;
        public int _damage;

        #endregion


        #region Hidden

        private Player _player;
        private Transform _transform;
        private Transform _healthSliderTransform;
        private NavMeshAgent _navMeshAgent;

        private bool _isPlayerInAttackRange = false;
        private bool _isAttacking = false;
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
            _player = _playerTransform.GetComponent<Player>();

            _navMeshAgent.speed = _movementSpeed;
            _navMeshAgent.acceleration = _movementSpeed;
            _currentHealth = _maxHealth;
            _initialPosition = _transform.position;

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

        public void TakeDamage(int damage)
        {
            if (_currentHealth - damage > 0)
            {
                StartCoroutine(DamageFeedback());
                _currentHealth -= damage;
                _healthSlider.value = ((float)_currentHealth/(float)_maxHealth);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion


        #region Utils

        IEnumerator AttackCoroutine()
        {
            while (_isPlayerInAttackRange == true)
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

        #endregion
    }
}
