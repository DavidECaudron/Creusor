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
        public Image _healthSlider;
        public GameObject _graphics;
        public GameObject _physics;
        public GameObject _healthBar;
        public GameObject _body;

        public GameObject _enemyProjectile;
        public Animator _animator;
        public Transform _projectileBin;
        public Transform _projectileSpawn;
        public Vector3 _initialPosition;
        public bool _isPlayerInDetectionRange = false;

        [Header("Enemy")]
        public EnemyType _enemyType;
        public int _level;
        public float _movementSpeed;
        public float _maxHealth;
        public float _currentHealth;
        public float _damageMelee;
        public float _damageRanged;
        public float _attackPerSecondMelee;
        public float _attackPerSecondRanged;
        public bool _isAlive = true;
        public bool _isHidden = false;

        //VFX
        public MeshRenderer _enemyBodyRend;
        public MeshRenderer _EnemyStoneRend;
        public MeshRenderer _EnemyParticleRend;
        public ParticleSystem _EnemyParticles;   
        ///
        public MeshRenderer _enemyMeleeMaskRend;   
        public MeshRenderer _EnemyMeleeTeethRend;
        public MeshRenderer _EnemyMeleeEyeLeftRend;
        public MeshRenderer _EnemyMeleeEyeRightRend;    
        public MeshRenderer _EnemyBiteAttackTopRend;  
        public MeshRenderer _EnemyBiteAttackBottomRend;    
        public MeshRenderer _enemyDistanceMaskRend;  
        public MeshRenderer _EnemyDistanceEyeRend;

        public CanvasGroup _healthCanvasGroup;

        public Transform _maskGroup;

        private Vector3 _initialMaskPos;
        private Quaternion _initialMaskRot;
        ///

        public AudioSource _audioSource;
        public AudioClip[] _attackMeleeBiteClips = new AudioClip[3]; 
        public AudioClip[] _DefeatClips = new AudioClip[4]; 

        public AudioClip[] _takeDamageClips = new AudioClip[2]; 
       //VFX
        #endregion


        #region Hidden

        private Player _player;
        private Transform _healthSliderTransform;
        private MeshRenderer _meshRenderer;
        private float _timeCheck;
        private bool _isPlayerInAttackRange = false;
        private bool _isAttacking = false;
        private int _heightIndex = 0;

        private bool isMoving = false;



        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _healthSliderTransform = _healthSlider.gameObject.transform.parent.parent.GetComponent<Transform>();
            _initialMaskPos = _maskGroup.transform.localPosition;
            _initialMaskRot = _maskGroup.transform.localRotation;
        }

        private void Start()
        {
            _timeCheck = Time.time;
            _initialPosition = _transform.position;
            _player = _playerTransform.GetComponent<Player>();
            _navMeshAgent.speed = _movementSpeed;
            _navMeshAgent.acceleration = _movementSpeed;
            _currentHealth = _maxHealth;
            _meshRenderer = _body.GetComponentInChildren<MeshRenderer>();

            if (_enemyType == EnemyType.Melee)
            {
                _enemyMeleeMaskRend.enabled = true;   
                _EnemyMeleeTeethRend.enabled = true;   
                _EnemyMeleeEyeLeftRend.enabled = true;   
                _EnemyMeleeEyeRightRend.enabled = true;      
                _EnemyBiteAttackTopRend.enabled = true;   
                _EnemyBiteAttackBottomRend.enabled = true;       
                _enemyDistanceMaskRend.enabled = false;     
                _EnemyDistanceEyeRend.enabled = false;    
            }

            if (_enemyType == EnemyType.Ranged)
            {
                _enemyMeleeMaskRend.enabled = false;   
                _EnemyMeleeTeethRend.enabled = false;   
                _EnemyMeleeEyeLeftRend.enabled = false;   
                _EnemyMeleeEyeRightRend.enabled = false;      
                _EnemyBiteAttackTopRend.enabled = false;   
                _EnemyBiteAttackBottomRend.enabled = false;       
                _enemyDistanceMaskRend.enabled = true;     
                _EnemyDistanceEyeRend.enabled = true;    
            }
        }

        private void LateUpdate()
        {
            Vector3 lookAtPosition = new (_transform.position.x, _cameraTransform.position.y, _cameraTransform.position.z);
            _healthSliderTransform.LookAt(lookAtPosition);

            Movement();
            if(!_player._isAlive)
            {
                _healthCanvasGroup.alpha = 0;
            }
        }

        #endregion


        #region Main

        public void Movement()
        {
            if (_isHidden == false)
            {
                if (_isAlive == true)
                {
                    Vector3 graphicsPosition = _graphics.transform.position;
                    Vector3 healthBarPosition = _healthBar.transform.position;

                    if (_heightIndex > 0)
                    {
                        _graphics.transform.position = new (graphicsPosition.x, -0.5f, graphicsPosition.z);
                        _healthBar.transform.position = new (healthBarPosition.x, 2.5f, healthBarPosition.z);
                    }
                    else
                    {
                        _graphics.transform.position = new (graphicsPosition.x, 1.0f, graphicsPosition.z);
                        _healthBar.transform.position = new (healthBarPosition.x, 3.5f, healthBarPosition.z);
                    }

                    if (_isPlayerInDetectionRange == true && _player._isAlive)
                    {
                        if (_enemyType == EnemyType.Melee)
                        {
                            if (Vector3.Distance(_playerTransform.position, _transform.position) > 2.0f)
                            {
                                _isPlayerInAttackRange = false;
                                _isAttacking = false;

                                StopCoroutine(AttackCoroutineMelee());

                                _navMeshAgent.SetDestination(_playerTransform.position);
                            }
                            else
                            {
                                _transform.LookAt(_playerTransform);

                                _navMeshAgent.SetDestination(_transform.position);
                                _isPlayerInAttackRange = true;

                                if (_isAttacking == false)
                                {
                                    _isAttacking = true;

                                    Collider[] hitColliders = Physics.OverlapSphere(_attackDetection.position, 0.5f);

                                    foreach (var hitCollider in hitColliders)
                                    {
                                        if (hitCollider.transform.CompareTag("player"))
                                        {
                                            StartCoroutine(AttackCoroutineMelee());
                                        }
                                    }
                                }
                            }
                        }

                        if (_enemyType == EnemyType.Ranged)
                        {
                            if (Vector3.Distance(_playerTransform.position, _transform.position) > 6.0f)
                            {
                                _isPlayerInAttackRange = false;
                                _isAttacking = false;

                                StopCoroutine(AttackCoroutineRanged());

                                _navMeshAgent.SetDestination(_playerTransform.position);
                            }
                            else
                            {
                                _transform.LookAt(_playerTransform);

                                _navMeshAgent.SetDestination(_transform.position);
                                _isPlayerInAttackRange = true;

                                if (_isAttacking == false)
                                {
                                    _isAttacking = true;

                                    StartCoroutine(AttackCoroutineRanged());
                                }
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
        }

        IEnumerator ResetMaskPosAndRot()
        {
            yield return new WaitForSecondsRealtime(3);
            _maskGroup.transform.localPosition = _initialMaskPos;
            _maskGroup.transform.localRotation = _initialMaskRot;     
            yield return null;     
        }

        public void TakeDamage(float damage)
        {

            if(_isAlive && damage > 0)
            {
                StartCoroutine(DamageFeedback());
                _audioSource.PlayOneShot(_takeDamageClips[Random.Range(0,_takeDamageClips.Length)], 0.3f);
            }

            if (_currentHealth > 0)
            {
                _currentHealth -= damage;
            
                if(_currentHealth <= 0)
                {
                    _healthSlider.fillAmount = 0;
                    _isAlive = false;
                    _EnemyParticles.Stop();
                    _player._isInAttackRange = false;
                    //HideEnemy();
                    _animator.SetBool("isDefeat", true);
                    int random = Random.Range(0,_DefeatClips.Length);
                    Debug.Log(random);
                    _audioSource.PlayOneShot(_DefeatClips[random], 0.3f);
                    StartCoroutine(ResetMaskPosAndRot());                    
                }
                else
                {
                    _healthSlider.fillAmount = (_currentHealth/_maxHealth);
                }
            }
        }

        public void Cursed()
        {
            StartCoroutine(CursedEnemy());
        }

        IEnumerator CursedEnemy()
        {
            float elapsedTime = 0;
            float duration = 1f;

            Vector3 currentBodyScale = _enemyBodyRend.transform.localScale;
            Vector3 targetBodyScale = Vector3.one;

            while(elapsedTime < duration)
            {
                float curseLerp = Mathf.Lerp(0, 1, elapsedTime / duration);
                Vector3 scaleLerp = Vector3.Lerp(currentBodyScale, targetBodyScale, elapsedTime / duration);

                _enemyBodyRend.material.SetFloat("_CurseAmount", curseLerp);
                _enemyMeleeMaskRend.material.SetFloat("_CurseAmount", curseLerp);   
                _enemyDistanceMaskRend.material.SetFloat("_CurseAmount", curseLerp);     
                _EnemyMeleeTeethRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyMeleeEyeLeftRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyMeleeEyeRightRend.material.SetFloat("_CurseAmount", curseLerp);      
                _EnemyDistanceEyeRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyStoneRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyBiteAttackTopRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyBiteAttackBottomRend.material.SetFloat("_CurseAmount", curseLerp);   
                _EnemyParticleRend.material.SetFloat("_CurseAmount", curseLerp);
                _enemyBodyRend.transform.localScale = scaleLerp;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _enemyBodyRend.material.SetFloat("_CurseAmount", 1);
            _enemyMeleeMaskRend.material.SetFloat("_CurseAmount", 1);   
            _enemyDistanceMaskRend.material.SetFloat("_CurseAmount", 1);     
            _EnemyMeleeTeethRend.material.SetFloat("_CurseAmount", 1);   
            _EnemyMeleeEyeLeftRend.material.SetFloat("_CurseAmount", 1);   
            _EnemyMeleeEyeRightRend.material.SetFloat("_CurseAmount", 1);      
            _EnemyDistanceEyeRend.material.SetFloat("_CurseAmount", 1);   
            _EnemyStoneRend.material.SetFloat("_CurseAmount", 1);   
            _EnemyBiteAttackTopRend.material.SetFloat("_CurseAmount", 1);
            _EnemyBiteAttackBottomRend.material.SetFloat("_CurseAmount", 1);      
            _EnemyParticleRend.material.SetFloat("_CurseAmount", 1);
            _enemyBodyRend.transform.localScale = targetBodyScale;     

            yield return null;
        }

        #endregion


        #region Utils

        public void HideEnemy()
        {
            _navMeshAgent.enabled = false;
            _graphics.transform.position = new (100.0f, 0.0f, 100.0f);
            _physics.transform.position = new (100.0f, 0.0f, 100.0f);
            _healthBar.transform.position = new (100.0f, 0.0f, 100.0f);
            _isHidden = true;
        }

        public void ShowEnemy()
        {
            _transform.position = _initialPosition;
            _navMeshAgent.enabled = true;
            _graphics.transform.localPosition = new (0.0f, 1.0f, 0.0f);
            _physics.transform.localPosition = new (0.0f, 1.0f, 0.0f);
            _healthBar.transform.localPosition = new (0.0f, 3.5f, 0.0f);
            _isHidden = false;
            _animator.SetBool("isDefeat", false);
        }

        IEnumerator AttackCoroutineMelee()
        {
            while (_isAlive == true && _isPlayerInAttackRange == true)
            {
                if (Time.time >= _timeCheck)
                {
                    _animator.SetTrigger("MeleeAttack");

                    _audioSource.PlayOneShot(_attackMeleeBiteClips[Random.Range(0,_attackMeleeBiteClips.Length)], 0.2f);
                    yield return new WaitForSeconds(0.2f);

                    _player.TakeDamage(_damageMelee);

                    _timeCheck = Time.time + (1 / _attackPerSecondMelee);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator AttackCoroutineRanged()
        {
            while (_isAlive == true && _isPlayerInAttackRange == true)
            {
                if (Time.time >= _timeCheck)
                {
                    GameObject clone = Instantiate(_enemyProjectile, _projectileSpawn.position, _projectileSpawn.rotation, _player._abilitiesClone);

                    clone.GetComponent<EnemyProjectile>()._damage = _damageRanged;

                    _timeCheck = Time.time + (1 / _attackPerSecondRanged);
                }

                yield return new WaitForEndOfFrame();
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

            //if (other.CompareTag("player"))
            //{
            //    if (_isAttacking == false)
            //    {
            //        StartCoroutine(AttackCoroutineRanged());

            //        _isAttacking = true;
            //    }
            //}
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

            //if (other.CompareTag("player"))
            //{
            //    _isAttacking = false;
            //}
        }

        #endregion
    }
}
