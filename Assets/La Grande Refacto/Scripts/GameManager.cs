using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private GameObject _playerGameObject;
        [SerializeField] private Camera _playerCamera;

        [SerializeField] private Transform _positionsTransform;

        [SerializeField] private LayerMask _mouseLeftButtonLayer;
        [SerializeField] private LayerMask _mouseRightButtonLayer;

        [Header("Debug")]
        [SerializeField] private Player _player;
        [SerializeField] private PlayerInput _playerInput;

        [SerializeField] private List<Position> _positionsList;
        [SerializeField] private List<EnemyPack> _enemyPackList;

        [SerializeField] private Vector3 _nextPosition;
        [SerializeField] private Vector3 _lookAtPosition;

        [SerializeField] private bool _leftButtonIndex = false;
        [SerializeField] private bool _rightButtonIndex = false;

        #endregion


        #region Updates

        //private void Awake()
        //{
            
        //}

        private void Start()
        {
            Setup();
            EnemySpawn();
        }

        private void Update()
        {
            PlayerMovement();
        }

        //private void FixedUpdate()
        //{

        //}

        //private void LateUpdate()
        //{

        //}

        #endregion


        #region Main

        private void Setup()
        {
            _player = _playerGameObject.GetComponent<Player>();
            _playerInput = _playerGameObject.GetComponent<PlayerInput>();

            _nextPosition = _player.GetTransform().position;
            _lookAtPosition = _player.GetTransform().position;

            _positionsList = new List<Position>();
            _enemyPackList = new List<EnemyPack>();
        }

        #endregion


        #region Player

        private void PlayerMovement()
        {
            _player.GetRigidbody().velocity = new(0.0f, 0.0f, 0.0f);

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

        #endregion


        #region Spawners

        private void EnemySpawn()
        {
            foreach (Transform item in _positionsTransform)
            {
                Position positionTemp = item.GetComponent<Position>();
                EnemyPack enemyPackTemp = item.GetComponent<EnemyPack>();

                positionTemp.GetSphereCollider().radius = positionTemp.GetRadius();

                _positionsList.Add(positionTemp);
                _enemyPackList.Add(enemyPackTemp);

                for (int i = 0; i < enemyPackTemp.GetNbMelee(); i++)
                {
                    float radiusTemp = positionTemp.GetRadius();

                    float randomX = Random.Range(-radiusTemp * 0.60f, radiusTemp * 0.60f);
                    float randomZ = Random.Range(-radiusTemp * 0.60f, radiusTemp * 0.60f);

                    float tempX = positionTemp.GetTransform().position.x + randomX;
                    float tempZ = positionTemp.GetTransform().position.z + randomZ;

                    Vector3 positionTmp = new (tempX, enemyPackTemp.GetEnemyScript().GetTransform().position.y, tempZ);

                    GameObject clone = Instantiate(enemyPackTemp.GetEnemy(), positionTmp, Quaternion.identity, positionTemp.GetTransform());
                    Enemy enemyClone = clone.GetComponent<Enemy>();

                    enemyPackTemp.GetEnemyList().Add(enemyClone);

                    enemyClone.SetEnemyType(EnemyType.Melee);
                    enemyClone.SetInitialPosition();
                    clone.SetActive(true);
                }

                for (int i = 0; i < enemyPackTemp.GetNbRanged(); i++)
                {
                    float radiusTemp = positionTemp.GetRadius();

                    float randomX = Random.Range(-radiusTemp * 0.60f, radiusTemp * 0.60f);
                    float randomZ = Random.Range(-radiusTemp * 0.60f, radiusTemp * 0.60f);

                    float tempX = positionTemp.GetTransform().position.x + randomX;
                    float tempZ = positionTemp.GetTransform().position.z + randomZ;

                    Vector3 positionTmp = new (tempX, enemyPackTemp.GetEnemyScript().GetTransform().position.y, tempZ);

                    GameObject clone = Instantiate(enemyPackTemp.GetEnemy(), positionTmp, Quaternion.identity, positionTemp.GetTransform());
                    Enemy enemyClone = clone.GetComponent<Enemy>();

                    enemyPackTemp.GetEnemyList().Add(enemyClone);

                    enemyClone.SetEnemyType(EnemyType.Ranged);
                    enemyClone.SetInitialPosition();
                    clone.SetActive(true);
                }
            }
        }

        #endregion


        #region Coroutine

        private IEnumerator PlayerNextPosition()
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

        private IEnumerator PlayerGraphicsLookAt()
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
