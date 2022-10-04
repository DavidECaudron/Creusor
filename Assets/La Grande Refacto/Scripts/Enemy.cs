using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LaGrandeRefacto.Root
{
    public enum EnemyType
    {
        Melee,
        Ranged
    }

    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private GameObject _enemyProjectile;
        [SerializeField] private Transform _enemyProjectileSpawn;
        [SerializeField] private Transform _projectileCloneBin;

        [Header("Tweak")]
        [SerializeField] private float _distanceMelee;
        [SerializeField] private float _distanceRanged;
        [SerializeField] private float _distanceInitialPosition;
        [SerializeField] private float _speed;
        [SerializeField] private float _meleeCooldown;
        [SerializeField] private float _rangedCooldown;

        [Header("Debug")]
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private float _timeCheck;
        [SerializeField] private bool _isPlayerInDetectionRange = false;
        [SerializeField] private bool _isPlayerInAttackRange = false;

        #endregion


        #region Updates

        private void Start()
        {
            _timeCheck = Time.time;
        }

        #endregion


        #region Coroutine

        public IEnumerator ChasePlayer(Player player)
        {
            while (_isPlayerInDetectionRange)
            {
                if (_enemyType == EnemyType.Melee)
                {
                    float distance = Vector3.Distance(_transform.position, player.GetTransform().position);

                    if (distance > _distanceMelee)
                    {
                        _navMeshAgent.SetDestination(player.GetTransform().position);
                    }
                    else
                    {
                        _navMeshAgent.SetDestination(_transform.position);
                        _transform.LookAt(player.GetTransform().position);
                    }
                }

                if (_enemyType == EnemyType.Ranged)
                {
                    float distance = Vector3.Distance(_transform.position, player.GetTransform().position);

                    if (distance > _distanceRanged)
                    {
                        if (_isPlayerInAttackRange)
                        {
                            _isPlayerInAttackRange = false;

                            StopCoroutine(AttackPlayerRanged());
                        }

                        _navMeshAgent.SetDestination(player.GetTransform().position);
                    }
                    else
                    {
                        _transform.LookAt(player.GetTransform().position);

                        if (!_isPlayerInAttackRange)
                        {
                            _isPlayerInAttackRange = true;

                            StartCoroutine(AttackPlayerRanged());
                        }

                        _navMeshAgent.SetDestination(_transform.position);
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator AttackPlayerRanged()
        {
            while (_isPlayerInAttackRange)
            {
                if (Time.time >= _timeCheck)
                {
                    GameObject clone = Instantiate(_enemyProjectile, _enemyProjectileSpawn.position, _enemyProjectileSpawn.rotation, _projectileCloneBin);
                    clone.SetActive(true);

                    _timeCheck = Time.time + _rangedCooldown;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator MoveToInitialPosition()
        {
            float distance = Vector3.Distance(_initialPosition, _transform.position);

            while (distance >= _distanceInitialPosition)
            {
                _navMeshAgent.SetDestination(_initialPosition);

                yield return new WaitForEndOfFrame();

                distance = Vector3.Distance(_initialPosition, _transform.position);
            }
        }

        #endregion


        #region Getter

        public Transform GetTransform()
        {
            return _transform;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return _navMeshAgent;
        }

        public EnemyType GetEnemyType()
        {
            return _enemyType;
        }

        public float GetSpeed()
        {
            return _speed;
        }

        public Vector3 GetInitialPosition()
        {
            return _initialPosition;
        }

        public float GetDistanceMelee()
        {
            return _distanceMelee;
        }

        public float GetDistanceRanged()
        {
            return _distanceRanged;
        }

        #endregion


        #region Setter

        public void SetEnemyType(EnemyType value)
        {
            _enemyType = value;
        }

        public void SetInitialPosition()
        {
            _initialPosition = _transform.position;
        }

        public void SetIsPlayerInRange(bool value)
        {
            _isPlayerInDetectionRange = value;
        }

        #endregion
    }
}
