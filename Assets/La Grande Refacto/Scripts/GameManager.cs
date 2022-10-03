using System.Collections;
using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private GameObject _playerGameObject;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private LayerMask _mouseLeftButtonLayer;
        [SerializeField] private LayerMask _mouseRightButtonLayer;

        [Header("Test")]
        [SerializeField] private GameObject _enemyGameObject;

        #endregion


        #region Hidden

        private Player _player;
        private PlayerInput _playerInput;

        private Enemy _enemy;

        private Vector3 _nextPosition;
        private Vector3 _lookAtPosition;

        private bool _leftButtonIndex = false;
        private bool _rightButtonIndex = false;

        #endregion


        #region Updates

        //private void Awake()
        //{
            
        //}

        private void Start()
        {
            _player = _playerGameObject.GetComponent<Player>();
            _playerInput = _playerGameObject.GetComponent<PlayerInput>();
            
            _enemy = _enemyGameObject.GetComponent<Enemy>();
            _enemy.GetNavMeshAgent().speed = _enemy.GetSpeed();
            _enemy.GetNavMeshAgent().acceleration = _enemy.GetSpeed();
            _enemy.GetNavMeshAgent().angularSpeed = _enemy.GetSpeed() * 100.0f;

            _nextPosition = _player.GetTransform().position;
            _lookAtPosition = _player.GetTransform().position;
        }

        private void Update()
        {
            if (_playerInput.GetMouseLeftButton())
            {
                if (!_leftButtonIndex)
                {
                    _leftButtonIndex = true;
                    StartCoroutine(PlayerNextPosition());
                }

                _player.Rotation(_nextPosition);
            }
            else
            {
                _leftButtonIndex = false;
            }

            if (_playerInput.GetMouseRightButton())
            {
                if (!_rightButtonIndex)
                {
                    StartCoroutine(PlayerGraphicsLookAt());
                    _rightButtonIndex = true;
                }

                _player.Rotation(_lookAtPosition);
            }
            else
            {
                _rightButtonIndex = false;
            }

            _player.Movement(_nextPosition);
        }

        //private void FixedUpdate()
        //{

        //}

        private void LateUpdate()
        {
            float distance = Vector3.Distance(_enemy.GetTransform().position, _player.GetTransform().position);

            if (_enemy.GetEnemyType() == EnemyType.Melee)
            {
                if (distance >= 2.0f)
                {
                    if (!_enemy.GetNavMeshAgent().enabled)
                    {
                        _enemy.GetNavMeshAgent().enabled = true;
                    }

                    _enemy.GetNavMeshAgent().SetDestination(_player.GetTransform().position);
                }
                else
                {
                    _enemy.GetTransform().LookAt(_player.GetTransform().position);

                    if (_enemy.GetNavMeshAgent().enabled)
                    {
                        _enemy.GetNavMeshAgent().enabled = false;
                    }
                }
            }

            if (_enemy.GetEnemyType() == EnemyType.Ranged)
            {
                if (distance >= 6.0f)
                {
                    if (!_enemy.GetNavMeshAgent().enabled)
                    {
                        _enemy.GetNavMeshAgent().enabled = true;
                    }

                    _enemy.GetNavMeshAgent().SetDestination(_player.GetTransform().position);
                }
                else
                {
                    _enemy.GetTransform().LookAt(_player.GetTransform().position);

                    if (_enemy.GetNavMeshAgent().enabled)
                    {
                        _enemy.GetNavMeshAgent().enabled = false;
                    }
                }
            }
        }

        #endregion


        #region Coroutine

        IEnumerator PlayerNextPosition()
        {
            while (_playerInput.GetMouseLeftButton())
            {
                Ray _ray = _playerCamera.ScreenPointToRay(_playerInput.GetMousePosition());

                Physics.Raycast(_ray, out RaycastHit value, Mathf.Infinity, _mouseLeftButtonLayer);
                Transform hit = value.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("ground"))
                    {
                        _nextPosition = value.point;
                    }

                    //if (hit.CompareTag("enemy"))
                    //{
                    //    _isTargetingEnemy = true;
                    //    _isTargetingGround = false;

                    //    _enemyTransform = _hit.transform;
                    //}
                }

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator PlayerGraphicsLookAt()
        {
            while (_playerInput.GetMouseRightButton())
            {
                Ray _ray = _playerCamera.ScreenPointToRay(_playerInput.GetMousePosition());

                Physics.Raycast(_ray, out RaycastHit value, Mathf.Infinity, _mouseRightButtonLayer);
                Transform hit = value.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("ground"))
                    {
                        _lookAtPosition = value.point;
                    }

                    //if (hit.CompareTag("enemy"))
                    //{
                    //    _isTargetingEnemy = true;
                    //    _isTargetingGround = false;

                    //    _enemyTransform = _hit.transform;
                    //}
                }

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion
    }
}
