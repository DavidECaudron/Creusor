using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace refacto_deux
{
    public class PlayerInput : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PlayerInputScriptableObject _playerInputScriptableObject;
        [SerializeField] private GameObject _digThrowDirtVisual;
        [SerializeField] private GameObject _digShockwaveVisual;
        [SerializeField] private float _attackPerSecond;
        [SerializeField] private Transform _position;
        [SerializeField] private Animator _animator;

        #endregion


        #region Hidden

        public bool _isEnemyInRange;
        public bool _isAttacking;

        #endregion


        #region Updates

        private void Start()
        {
            _playerInputScriptableObject.IsLeftClicking = false;
            _isEnemyInRange = false;
            _isAttacking = false;
            _animator.gameObject.SetActive(false);
        }

        #endregion


        #region Main

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            _playerInputScriptableObject.MousePosition = context.ReadValue<Vector2>();
        }

        public void OnMouseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInputScriptableObject.IsLeftClicking = true;

                StartCoroutine(CastNextPosition());
                if (_isAttacking == false)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }

            if (context.canceled)
            {
                _playerInputScriptableObject.IsLeftClicking = false;
            }
        }

        public void OnDigThrowDirt(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInputScriptableObject.PlayerMovementScriptableObject.IsUsingSpell = true;

                _digThrowDirtVisual.SetActive(true);

                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                if (_playerInputScriptableObject.DigThrowDirtScriptableObject.TimeCheck + _playerInputScriptableObject.DigThrowDirtScriptableObject.CooldownInSeconds <= Time.time)
                {
                    _playerInputScriptableObject.PlayerAbilitiesScriptableObject.AbilityList[0].UseAbility();
                }

                _digThrowDirtVisual.SetActive(false);

                _playerInputScriptableObject.PlayerMovementScriptableObject.IsUsingSpell = false;
            }
        }

        //public void OnTunnelDash(InputAction.CallbackContext context)
        //{
        //    if (context.performed)
        //    {
        //        PlayerMovement.IsUsingSpell = true;
        //        StartCoroutine(CastNextPosition());
        //    }

        //    if (context.canceled)
        //    {
        //        PlayerSpells.Spell(1);
        //        PlayerMovement.IsUsingSpell = false;
        //    }
        //}

        //public void OnDigDrill(InputAction.CallbackContext context)
        //{
        //    if (context.performed)
        //    {
        //        PlayerMovement.IsUsingSpell = true;
        //        StartCoroutine(CastNextPosition());
        //    }

        //    if (context.canceled)
        //    {
        //        PlayerSpells.Spell(2);
        //        PlayerMovement.IsUsingSpell = false;
        //    }
        //}

        public void OnDigShockwave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playerInputScriptableObject.PlayerMovementScriptableObject.IsUsingSpell = true;

                _digShockwaveVisual.SetActive(true);

                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                if (_playerInputScriptableObject.DigShockwaveScriptableObject.TimeCheck + _playerInputScriptableObject.DigShockwaveScriptableObject.CooldownInSeconds <= Time.time)
                {
                    _playerInputScriptableObject.PlayerAbilitiesScriptableObject.AbilityList[1].UseAbility();
                }

                _digShockwaveVisual.SetActive(false);

                _playerInputScriptableObject.PlayerMovementScriptableObject.IsUsingSpell = false;
            }
        }

        #endregion


        #region Utils

        private void FromCameraToWorldPoint()
        {
            RaycastHit value;

            _playerInputScriptableObject.Ray = _playerInputScriptableObject.Camera.ScreenPointToRay(_playerInputScriptableObject.MousePosition);
            Physics.Raycast(_playerInputScriptableObject.Ray, out value);

            _playerInputScriptableObject.RaycastHit = value;

            if (_playerInputScriptableObject.IsLeftClicking == true)
            {
                SetNextPosition();
            }
            else
            {
                SetLookAtMousePosition();
            }
        }

        private void SetNextPosition()
        {
            if (_playerInputScriptableObject.RaycastHit.transform != null)
            {
                if (_playerInputScriptableObject.RaycastHit.transform.CompareTag("ground"))
                {
                    _playerInputScriptableObject.PlayerMovementScriptableObject.NextPosition = new Vector3(_playerInputScriptableObject.RaycastHit.point.x, 
                                                                                                           _playerInputScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y, 
                                                                                                           _playerInputScriptableObject.RaycastHit.point.z);
                }

                if (_playerInputScriptableObject.RaycastHit.transform.CompareTag("enemy"))
                {
                    _playerInputScriptableObject.PlayerMovementScriptableObject.NextPosition = new Vector3(_playerInputScriptableObject.RaycastHit.transform.position.x,
                                                                                                           _playerInputScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y, 
                                                                                                           _playerInputScriptableObject.RaycastHit.transform.position.z);
                }
            }
        }

        private void SetLookAtMousePosition()
        {
            if (_playerInputScriptableObject.RaycastHit.transform != null)
            {
                if (_playerInputScriptableObject.RaycastHit.transform.CompareTag("ground"))
                {
                    _playerInputScriptableObject.PlayerMovementScriptableObject.LookPosition = new Vector3(_playerInputScriptableObject.RaycastHit.point.x, 
                                                                                                           _playerInputScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y, 
                                                                                                           _playerInputScriptableObject.RaycastHit.point.z);
                }

                if (_playerInputScriptableObject.RaycastHit.transform.CompareTag("enemy"))
                {
                    _playerInputScriptableObject.PlayerMovementScriptableObject.LookPosition = new Vector3(_playerInputScriptableObject.RaycastHit.transform.position.x,
                                                                                                           _playerInputScriptableObject.PlayerScriptableObject.PlayerRigidbody.position.y, 
                                                                                                           _playerInputScriptableObject.RaycastHit.transform.position.z);
                }
            }
        }

        IEnumerator CastNextPosition()
        {
            while (_playerInputScriptableObject.IsLeftClicking == true || 
                   _playerInputScriptableObject.PlayerMovementScriptableObject.IsUsingSpell == true)
            {
                FromCameraToWorldPoint();

                yield return new WaitForFixedUpdate();
            }
        }

        IEnumerator AttackCoroutine()
        {
            _isAttacking = true;
            _animator.gameObject.SetActive(true);

            while (_isEnemyInRange == true)
            {
                yield return new WaitForSeconds(1.0f / _attackPerSecond);

                _animator.Play("Shovel_001_ComboStep_01");

                Collider[] hitColliders = Physics.OverlapSphere(_position.position, 0.5f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.transform.CompareTag("enemy"))
                    {
                        hitCollider.transform.parent.parent.GetComponent<Enemy>().TakeDamage(20);
                    }
                }
            }

            _animator.gameObject.SetActive(false);
            _isAttacking = false;
        }

        #endregion
    }
}
