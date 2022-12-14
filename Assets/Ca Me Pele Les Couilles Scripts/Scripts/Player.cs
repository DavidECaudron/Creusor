using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using proto;

namespace caca
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Game_Manager _gameManager;
        public Camera _camera;
        public Transform _abilitiesClone;
        public Transform _graphics;
        public Transform _physics;
        public Transform _shovelTransform;
        public TMP_Text _healthCounter;
        public TMP_Text _coconutCounter;
        public CanvasGroup _healItemCanvasGroup;
        public CanvasGroup _healItemCursedCanvasGroup;
        public Animator _healItemAnimator;
        public AudioSource _healAudioSource;
        public AudioClip _healAudioClip;
        public AudioClip _getCoconutClip;              
        public Image _healthImage;
        public Image _healthCursedImage;
        public Image _healthImageBackground;
        public Animator _animator;
        public LayerMask _leftMouseButtonLayerMask;
        public LayerMask _abilitiesLayerMask;
        public LayerMask _graphicsHeightLayer;
        public bool _isInAttackRange = false;
        public bool _isCursed = false;
        public AnimationScriptable _animationScriptable;

        [Header("Player")]
        public int _nbCoconut = 0;
        [Range(0.0f, 1.0f)] public float _healthRegenPercentile;
        public float _movementSpeed;
        public float _maxHealth;
        public float _healthTimeBeforeUpdate;
        [Range(0.0f, 1.0f)] public float _healthBackgroundSpeed;
        public float _slowIntensity;
        public float _slowDuration;
        public bool _isAlive = true;

        [Header("Left Button")]
        public Image _leftButtonCooldownImage;
        public int _leftButtonDamage;
        public float _leftButtonCooldown;

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

        private Vector2 _mousePosition = new (0.0f, 0.0f);
        private Vector3 _nextPosition = new (0.0f, 0.0f, 0.0f);
        private Vector3 _lookPosition = new (0.0f, 0.0f, 0.0f);
        private Ray _ray;
        private RaycastHit _hit;

        private bool _isLeftClicking = false;
        private bool _isUsingAbility = false;
        
        private bool _isTargetingGround = false;
        private bool _isTargetingEnemy = false;
        private bool _isCalculatingHealth = false;
        private bool _hasBeenHit = false;
        private float _timeCheckLeftButton = 0.0f;
        private float _timeCheckRightButton = 0.0f;
        private float _timeCheckShockwave = 0.0f;
        private float _currentHealth;
        private int _heightIndex = 0;

        //VFX
        public SkinnedMeshRenderer _playerMeshRenderer;
        public MeshRenderer _shovelMeshRenderer;
        //VFX

        //SFX

        public AudioSource audioSourcePlayer;
        public AudioClip[] playerDamageClips = new AudioClip[4];


        //SFX

        #endregion


        #region Updates

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _transform = gameObject.GetComponent<Transform>();
            _animationScriptable._player = this;
        }

        private void Start()
        {
            StartCoroutine(PlayerGroundDetection());
            _timeCheckLeftButton = -_leftButtonCooldown;
            _timeCheckRightButton = -_rightButtonCooldown;
            _timeCheckShockwave = -_shockwaveCooldown;
            _currentHealth = _maxHealth;
            _healthCounter.text = _currentHealth + " / " + _maxHealth;
            //_coconutCounter.text = _nbCoconut.ToString();
            _healItemCanvasGroup.alpha = 0;
        }

        private void Update()
        {

            if(!_isAlive || _gameManager._inDialogue)
            {
                _nextPosition = _transform.position;
                _animator.SetBool("_isRunning", false);

            }

            if(_isAlive && !_gameManager._inDialogue)
            {
                Movement();

            }
            if ((Time.time - _timeCheckLeftButton) / _leftButtonCooldown <= 1.05f)
            {
                _leftButtonCooldownImage.fillAmount = (Time.time - _timeCheckLeftButton) / _leftButtonCooldown;
            }
            else
            {
                _leftButtonCooldownImage.fillAmount = 0.0f;
            }

            if ((Time.time - _timeCheckRightButton) / _rightButtonCooldown <= 1.05f)
            {
                _rightButtonCooldownImage.fillAmount = (Time.time - _timeCheckRightButton) / _rightButtonCooldown;
            }
            else
            {
                _rightButtonCooldownImage.fillAmount = 0.0f;
            }

            if ((Time.time - _timeCheckShockwave) / _shockwaveCooldown <= 1.05f)
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

            Vector3 graphicsPosition = _graphics.transform.position;

            if (_heightIndex > 0)
            {
                _graphics.transform.position = new Vector3(graphicsPosition.x, -1.0f, graphicsPosition.z);
            }
            else
            {
                _graphics.transform.position = new Vector3(graphicsPosition.x, 0.0f, graphicsPosition.z);
            }

            if (_animator.GetBool("_isShockwaving") == true)
            {
                _nextPosition = _transform.position;
            }

            Vector3 direction;

            if (_isTargetingGround == true && Vector3.Distance(_transform.position, _nextPosition) > 1.0f)
            {
                _animator.SetBool("_isRunning", true);

                direction = _nextPosition - _transform.position;

                _transform.Translate(_movementSpeed * Time.deltaTime * direction.normalized);
            }
            else
            {
                _animator.SetBool("_isRunning", false);
            }

            if (_isTargetingEnemy == true)
            {
                if (_enemyTransform != null)
                {
                    _nextPosition.x = _enemyTransform.position.x;
                    _nextPosition.z = _enemyTransform.position.z;
                }

                if (Vector3.Distance(_nextPosition, _transform.position) > 2.0f)
                {
                    _isInAttackRange = false;

                    _animator.SetBool("_isRunning", true);

                    direction = _nextPosition - _transform.position;

                    _transform.Translate(_movementSpeed * Time.deltaTime * direction.normalized);
                }
                else
                {
                    _isInAttackRange = true;

                    _animator.SetBool("_isRunning", false);
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

        public void TakeDamage(float damage)
        {
            damage = Mathf.RoundToInt(damage);

            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
                _healthImage.fillAmount = (_currentHealth / _maxHealth);
                _healthCursedImage.fillAmount = (_currentHealth / _maxHealth);                
                _healthCounter.text = _currentHealth + " / " + _maxHealth;

                audioSourcePlayer.PlayOneShot(playerDamageClips[Random.Range(0, playerDamageClips.Length)], 0.2f);

                StartCoroutine(DamageFeedback());
                StartCoroutine(SlowCoroutine());
                StartCoroutine(HealthBarCoroutine());

                _hasBeenHit = true;
            }
            else
            {
                if(_isAlive)
                {
                    _isAlive = false;
                    _animator.SetTrigger("_IsDead");
                    _gameManager.GameOver();
                }
            }
        }

        public void CurseConsomable()
        {
            if(_nbCoconut > 0)
            {
                _healItemCursedCanvasGroup.alpha = 1;
                _healItemAnimator.SetTrigger("CurseHealingItem");
            }
        }

        #endregion


        #region Input System

        public void OnMouseLeftButton(InputAction.CallbackContext context)
        {
            if (context.performed  && _isAlive && !_gameManager._inDialogue)
            {
                _isLeftClicking = true;

                StartCoroutine(CastNextPositionCoroutine());

                if (_isInAttackRange == true && _enemyTransform != null)
                {
                    if (Time.time >= _leftButtonCooldown + _timeCheckLeftButton && _animator.GetBool("_isSlashing") == false)
                    {
                        _animator.SetBool("_isSlashing", true);

                        _enemyTransform.GetComponent<Enemy>().TakeDamage(_leftButtonDamage);

                        _timeCheckLeftButton = Time.time;
                    }
                }
            }

            if (context.canceled)
            {
                _isLeftClicking = false;
            }
        }

        public void OnMouseRightButton(InputAction.CallbackContext context)
        {
            if (context.performed && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded)
            {
                _isUsingAbility = true;

                StartCoroutine(CastLookPositionCoroutine());

                _rightButtonPivot.gameObject.SetActive(true);
            }

            if (context.canceled && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded)
            {
                _rightButtonPivot.gameObject.SetActive(false);

                if (Time.time >= _rightButtonCooldown + _timeCheckRightButton)
                {
                    GameObject clone = Instantiate(_rightButtonPrefab, _rightButtonSpawn.position, _rightButtonSpawn.rotation, _abilitiesClone);

                    Collider[] hitColliders = Physics.OverlapSphere(_rightButtonSpawn.position, 1.0f);

                    foreach (Collider hit in hitColliders)
                    {
                        if (hit.CompareTag("chest"))
                        {
                            Chest chest = hit.transform.GetComponent<Chest>();

                            if (chest._isTaken == false)
                            {
                                _gameManager.AddGold(chest._gold);
                                _gameManager.AddChest(chest._nbChest);

                                if (chest._isTrapped == true)
                                {
                                    _gameManager._chestPackTable[chest._indexChestPack].UnTrapChest();
                                    _gameManager._enemyPackTable[chest._indexEnemyPack].ShowEnemy();
                                }

                                if(chest._mask != null)
                                {
                                    chest._mask.SetActive(true);
                                }

                                if (chest._areaMask != null)
                                {
                                    chest._areaMask.SetActive(false);
                                }

                                chest._isTaken = true;

                                if (chest._animator != null)
                                {
                                    chest._animator.SetBool("RevealChest", true);
                                    chest._itemSpawner.DropCoins();
                                }
                            }
                        }

                        if (hit.CompareTag("retrievable"))
                        {
                            Chest chest = hit.transform.GetComponent<Chest>();

                            if (chest._isTaken == false)
                            {
                                _gameManager.AddGold(chest._gold);

                                if (chest._isTrapped == true)
                                {
                                    _gameManager._chestPackTable[chest._indexChestPack].UnTrapChest();
                                    _gameManager._enemyPackTable[chest._indexEnemyPack].ShowEnemy();
                                }

                                if (chest._mask != null)
                                {
                                    chest._mask.SetActive(true);
                                }

                                if (chest._areaMask != null)
                                {
                                    chest._areaMask.SetActive(false);
                                }

                                Destroy(hit.gameObject, 2.0f);
                            }
                        }

                        if (hit.CompareTag("enemy"))
                        {
                            hit.transform.parent.parent.GetComponent<Enemy>().TakeDamage(_rightButtonDamage);
                        }

                        if (hit.CompareTag("destructible"))
                        {
                            //hit.gameObject.SetActive(false);

                            TreeBehavior treeBehavior = hit.GetComponent<TreeBehavior>();

                            if (treeBehavior != null)
                            {
                                treeBehavior.DestroyTree(clone.transform.position);
                            }
                            else
                            {
                                hit.gameObject.SetActive(false);
                            }
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
            if (context.performed  && _isAlive && !_gameManager._inDialogue)
            {
                _mousePosition = context.ReadValue<Vector2>();
            }

            if (context.canceled)
            {

            }
        }

        public void OnDig(InputAction.CallbackContext context)
        {
            if (context.performed )
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
            if (context.performed  && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded)
            {
                _shockwaveSpawn.gameObject.SetActive(true);
            }

            if (context.canceled  && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded)
            {
                _shockwaveSpawn.gameObject.SetActive(false);

                if (Time.time >= _shockwaveCooldown + _timeCheckShockwave)
                {
                    _animator.SetBool("_isShockwaving", true);

                    _timeCheckShockwave = Time.time;
                }
            }
        }

        public void OnConsumable(InputAction.CallbackContext context)
        {
            if (context.performed && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded)
            {
                if (!_isCursed)
                {
                    if (_nbCoconut > 0)
                    {
                        if (_currentHealth + _maxHealth * _healthRegenPercentile < _maxHealth)
                        {
                            _currentHealth += _maxHealth * _healthRegenPercentile;

                            _healthImage.fillAmount = (_currentHealth / _maxHealth);

                            _healthCursedImage.fillAmount = (_currentHealth / _maxHealth);
                            _healthCounter.text = _currentHealth + " / " + _maxHealth;
                            _healthImageBackground.fillAmount = (_currentHealth / _maxHealth);

                            _nbCoconut -= 1;
                            _healItemAnimator.SetTrigger("UseHealingItem");
                            _healAudioSource.PlayOneShot(_healAudioClip, 0.3f);
                            _coconutCounter.text = _nbCoconut.ToString();

                            if(_nbCoconut > 0 && _nbCoconut < 2)
                            {
                                _healItemCanvasGroup.alpha = 1;
                            }
                            else if( _nbCoconut == 0)
                            {
                                StartCoroutine(HideCoconutIcon());
                            }
                        }
                        else
                        {
                            if (_currentHealth < _maxHealth)
                            {
                                _currentHealth = _maxHealth;

                                _healthImage.fillAmount = (_maxHealth / _maxHealth);
                                _healthCursedImage.fillAmount = (_maxHealth / _maxHealth);
                                _healthCounter.text = _currentHealth + " / " + _maxHealth;
                                _healthImageBackground.fillAmount = (_maxHealth / _maxHealth);

                                _nbCoconut -= 1;
                                _healAudioSource.PlayOneShot(_healAudioClip, 0.3f);
                                _coconutCounter.text = _nbCoconut.ToString();
                                _healItemAnimator.SetTrigger("UseHealingItem");

                                if(_nbCoconut > 0 && _nbCoconut < 2)
                                {
                                    _healItemCanvasGroup.alpha = 1;
                                }
                                else if( _nbCoconut == 0)
                                {
                                    StartCoroutine(HideCoconutIcon());
                                }
                            }
                        }
                    }
                }
            }

            if (context.canceled)
            {

            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && _isAlive && !_gameManager._inDialogue && _gameManager._introIsEnded && _gameManager._nearOfPelican)
            {
                _gameManager.NewDialogue();
            }

            if (context.canceled)
            {

            }
        }

        IEnumerator HideCoconutIcon()
        {
            yield return new WaitForSeconds(0.3f);
            _healItemCanvasGroup.alpha = 0;
            yield return null;
        }

        IEnumerator DamageFeedback()
        {
            _animator.SetBool("_TakeDamage", true);
            _playerMeshRenderer.material.SetFloat("_DamageColorAmount", 1.0f);
            _shovelMeshRenderer.material.SetFloat("_DamageColorAmount", 1.0f);

            yield return new WaitForSecondsRealtime(0.10f);

            _playerMeshRenderer.material.SetFloat("_DamageColorAmount", 0f);
            _shovelMeshRenderer.material.SetFloat("_DamageColorAmount", 0f);
            _animator.SetBool("_TakeDamage", false);
        }

        #endregion


        #region Utils

        IEnumerator CastNextPositionCoroutine()
        {
            while (_isLeftClicking == true)
            {
                _ray = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_ray, out RaycastHit value, Mathf.Infinity, _leftMouseButtonLayerMask);

                _hit = value;

                Transform hit = _hit.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("ground"))
                    {
                        _isTargetingGround = true;
                        _isTargetingEnemy = false;

                        _enemyTransform = null;
                        _isInAttackRange = false;

                        _nextPosition.x = _hit.point.x;
                        _nextPosition.z = _hit.point.z;
                    }

                    if (hit.CompareTag("enemy"))
                    {
                        _isTargetingEnemy = true;
                        _isTargetingGround = false;

                        _enemyTransform = _hit.transform;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator CastLookPositionCoroutine()
        {
            while (_isUsingAbility == true)
            {
                _ray = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_ray, out RaycastHit value, Mathf.Infinity, _abilitiesLayerMask);

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

        IEnumerator SlowCoroutine()
        {
            if (_hasBeenHit == false)
            {
                _movementSpeed -= _slowIntensity;

                yield return new WaitForSecondsRealtime(_slowDuration);

                _movementSpeed += _slowIntensity;

                _hasBeenHit = false;
            }
        }

        IEnumerator HealthBarCoroutine()
        {
            if (_isCalculatingHealth == false)
            {
                _isCalculatingHealth = true;

                yield return new WaitForSecondsRealtime(_healthTimeBeforeUpdate);

                while (_healthImageBackground.fillAmount > _healthImage.fillAmount)
                {
                    _healthImageBackground.fillAmount -= _healthBackgroundSpeed * Time.deltaTime;

                    yield return new WaitForSecondsRealtime(0.01f);
                }

                _isCalculatingHealth = false;
            }
        }

        private IEnumerator PlayerGroundDetection()
        {
            while (true)
            {
                Physics.Raycast(_physics.position, new Vector3(0.0f, -1.0f, 0.0f), out RaycastHit value, 5.0f, _graphicsHeightLayer);
                Transform hit = value.transform;

                if (hit != null)
                {
                    if (hit.CompareTag("height"))
                    {
                        Vector3 temp = new(_graphics.localPosition.x, -1.0f, _graphics.localPosition.z);

                        _graphics.localPosition = temp;
                    }
                    else
                    {
                        Vector3 temp = new(_graphics.localPosition.x, 0.0f, _graphics.localPosition.z);

                        _graphics.localPosition = temp;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        //IEnumerator ShockwaveCoroutine()
        //{
        //    _animator.SetBool("_isShockwaving", true);

        //    yield return new WaitForSecondsRealtime(0.5f);

        //    _animator.SetBool("_isShockwaving", false);
        //}

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

            if (other.CompareTag("coconut"))
            {
                _healAudioSource.PlayOneShot(_getCoconutClip, 0.25f);
                _healItemAnimator.SetTrigger("GetCoconut");
                if(_isCursed)
                {
                    _healItemCursedCanvasGroup.alpha = 1;
                    _healItemCanvasGroup.alpha = 1;    
                    _healItemAnimator.SetTrigger("CurseHealingItem");                
                }
                else
                {
                    _nbCoconut += 1;
                    _coconutCounter.text = _nbCoconut.ToString();

                    if(_nbCoconut > 0 && _nbCoconut < 2)
                    {
                        _healItemCanvasGroup.alpha = 1;
                    }
                }

                Destroy(other.gameObject);
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
