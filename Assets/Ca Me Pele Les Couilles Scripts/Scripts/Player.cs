using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace caca
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Camera _camera;
        public Transform _abilitiesClone;
        public Transform _graphics;
        public LayerMask _rightMouseButtonLayerMask;
        public LayerMask _abilitiesLayerMask;

        [Header("Player")]
        [Range(0.0f, 10.0f)] public int _movementSpeed;

        [Header("Right Button")]
        public GameObject _rightButtonPrefab;
        public Transform _rightButtonPivot;
        public Transform _rightButtonSpawn;
        public Image _rightButtonCooldownImage;
        public int _rightButtonDamage;
        public float _rightButtonCooldown;

        [Header("Shockwave")]
        public GameObject _shockwavePrefab;
        public Transform _shockwaveSpawn;
        public Image _shockwaveCooldownImage;
        public int _shockwaveDamage;
        public float _shockwaveCooldown;
        
        #endregion


        #region Hidden

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Transform _enemyTransform;

        private Vector2 _mousePosition = new Vector2(0.0f, 0.0f);
        private Vector3 _nextPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 _lookPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private Ray _ray;
        private RaycastHit _hit;

        private bool _isLeftClicking = false;
        private bool _isUsingAbility = false;
        private bool _isTargetingGround = false;
        private bool _isTargetingEnemy = false;
        private float _timeCheckRightButton = 0.0f;
        private float _timeCheckShockwave = 0.0f;

        #endregion


        #region Updates

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _transform = gameObject.GetComponent<Transform>();
        }

        private void Start()
        {
            _timeCheckRightButton = -_rightButtonCooldown;
            _timeCheckShockwave = -_shockwaveCooldown;
        }

        private void Update()
        {
            Movement();

            if ((Time.time - _timeCheckRightButton) / _rightButtonCooldown <= 1.1f)
            {
                _rightButtonCooldownImage.fillAmount = (Time.time - _timeCheckRightButton) / _rightButtonCooldown;
            }
            else
            {
                _rightButtonCooldownImage.fillAmount = 0.0f;
            }

            if ((Time.time - _timeCheckShockwave) / _shockwaveCooldown <= 1.1f)
            {
                _shockwaveCooldownImage.fillAmount = (Time.time - _timeCheckShockwave) / _shockwaveCooldown;
            }
            else
            {
                _shockwaveCooldownImage.fillAmount = 0.0f;
            }
        }

        #endregion


        #region Main

        public void Movement()
        {
            if (_rigidbody.velocity != Vector3.zero)
            {
                _rigidbody.velocity = Vector3.zero;
            }

            Vector3 direction;

            if (_isTargetingGround == true && Vector3.Distance(_transform.position, _nextPosition) > 1.0f)
            {
                direction = _nextPosition - _transform.position;

                _transform.Translate(direction.normalized * _movementSpeed * Time.deltaTime);
            }

            if (_isTargetingEnemy == true)
            {
                if (_enemyTransform != null)
                {
                    _nextPosition.x = _enemyTransform.position.x;
                    _nextPosition.z = _enemyTransform.position.z;
                }

                if (Vector3.Distance(_transform.position, _nextPosition) > 2.0f)
                {
                    direction = _nextPosition - _transform.position;

                    _transform.Translate(direction.normalized * _movementSpeed * Time.deltaTime);
                }
            }

            if (_isLeftClicking == true)
            {
                _graphics.LookAt(new Vector3(_nextPosition.x, _graphics.position.y, _nextPosition.z));
            }

            if (_isUsingAbility == true)
            {
                _graphics.LookAt(new Vector3(_lookPosition.x, _graphics.position.y, _lookPosition.z));
                _rightButtonPivot.LookAt(new Vector3(_lookPosition.x, _rightButtonPivot.position.y, _lookPosition.z));
            }
        }

        #endregion


        #region Input System

        public void OnMouseLeftButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isLeftClicking = true;
                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                _isLeftClicking = false;
            }
        }

        public void OnMouseRightButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isUsingAbility = true;

                StartCoroutine(CastLookPosition());

                _rightButtonPivot.gameObject.SetActive(true);
            }

            if (context.canceled)
            {
                _rightButtonPivot.gameObject.SetActive(false);

                if (Time.time >= _rightButtonCooldown + _timeCheckRightButton)
                {
                    Instantiate(_rightButtonPrefab, _rightButtonSpawn.position, _rightButtonSpawn.rotation, _abilitiesClone);

                    Collider[] hitColliders = Physics.OverlapSphere(_rightButtonSpawn.position, 1.0f);

                    foreach (Collider hit in hitColliders)
                    {
                        //if (hit.CompareTag("chest"))
                        //{
                        //    if (hit.transform.parent.parent.GetComponent<Chest>()._isChestTaken == false)
                        //    {
                        //        _gameManagerScriptableObject.GameManager.AddGold();

                        //        hitCollider.transform.parent.GetComponent<Chest>()._isChestTaken = true;

                        //        Destroy(hit.transform.parent.gameObject, 2.0f);
                        //    }
                        //}

                        if (hit.CompareTag("enemy"))
                        {
                            hit.transform.parent.parent.GetComponent<Enemy>().TakeDamage(_rightButtonDamage);
                        }

                        //if (hit.CompareTag("ground"))
                        //{
                        //    Debug.Log("ground");
                        //}
                    }

                    _timeCheckRightButton = Time.time;
                }

                _isUsingAbility = false;
            }
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _mousePosition = context.ReadValue<Vector2>();
            }

            if (context.canceled)
            {

            }
        }

        public void OnDig(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                
            }

            if (context.canceled)
            {
                
            }
        }

        public void OnTunnel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }

            if (context.canceled)
            {

            }
        }

        public void OnShockwave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _shockwaveSpawn.gameObject.SetActive(true);
            }

            if (context.canceled)
            {
                _shockwaveSpawn.gameObject.SetActive(false);

                if (Time.time >= _shockwaveCooldown + _timeCheckShockwave)
                {
                    Instantiate(_shockwavePrefab, _shockwaveSpawn.position, _shockwaveSpawn.rotation, _abilitiesClone);

                    Collider[] hitColliders = Physics.OverlapSphere(_shockwaveSpawn.position, 4.0f);

                    foreach (Collider hit in hitColliders)
                    {
                        //if (hit.CompareTag("chest"))
                        //{
                        //    if (hit.transform.parent.parent.GetComponent<Chest>()._isChestTaken == false)
                        //    {
                        //        _gameManagerScriptableObject.GameManager.AddGold();

                        //        hitCollider.transform.parent.GetComponent<Chest>()._isChestTaken = true;

                        //        Destroy(hit.transform.parent.gameObject, 2.0f);
                        //    }
                        //}

                        if (hit.CompareTag("enemy"))
                        {
                            hit.transform.parent.parent.GetComponent<Enemy>().TakeDamage(_shockwaveDamage);
                        }

                        //if (hit.CompareTag("ground"))
                        //{
                        //    Debug.Log("ground");
                        //}
                    }

                    _timeCheckShockwave = Time.time;
                }
            }
        }

        #endregion


        #region Utils

        IEnumerator CastNextPosition()
        {
            while (_isLeftClicking == true)
            {
                RaycastHit value;

                _ray = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_ray, out value, Mathf.Infinity, _rightMouseButtonLayerMask);

                _hit = value;

                Transform hit = _hit.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("ground"))
                    {
                        _isTargetingGround = true;
                        _isTargetingEnemy = false;

                        _nextPosition.x = _hit.point.x;
                        _nextPosition.z = _hit.point.z;
                    }

                    if (hit.CompareTag("enemy"))
                    {
                        _isTargetingEnemy = true;
                        _isTargetingGround = false;

                        _enemyTransform = _hit.transform.parent.parent;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator CastLookPosition()
        {
            while (_isUsingAbility == true)
            {
                RaycastHit value;

                _ray = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_ray, out value, Mathf.Infinity, _abilitiesLayerMask);

                _hit = value;

                Transform hit = _hit.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("ground"))
                    {
                        _lookPosition.x = _hit.point.x;
                        _lookPosition.z = _hit.point.z;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        #endregion
    }
}