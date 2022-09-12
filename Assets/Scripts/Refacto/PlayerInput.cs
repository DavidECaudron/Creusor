using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace refacto
{
    public class PlayerInput : MonoBehaviour
    {
        #region Inspector
        #endregion

        #region Hidden

        private Player _player;
        private PlayerSpells _playerSpells;
        private PlayerMovement _playerMovement;

        private Ray _ray;
        private RaycastHit _raycasthit;

        private bool _isLeftClicking;

        #endregion

        #region Get Set

        public Player Player { get => _player; private set => _player = value; }
        public PlayerSpells PlayerSpells { get => _playerSpells; private set => _playerSpells = value; }
        public PlayerMovement PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }

        public Ray Ray { get => _ray; private set => _ray = value; }
        public RaycastHit RaycastHit { get => _raycasthit; private set => _raycasthit = value; }

        public bool IsLeftClicking { get => _isLeftClicking; private set => _isLeftClicking = value; }

        #endregion

        #region Updates

        private void Awake()
        {
            Player = gameObject.GetComponent<Player>();
            PlayerSpells = gameObject.GetComponent<PlayerSpells>();
            PlayerMovement = gameObject.GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            IsLeftClicking = false;
        }

        #endregion

        #region Main

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            Player.MousePosition = context.ReadValue<Vector2>();
        }

        public void OnMouseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsLeftClicking = true;

                StartCoroutine(CastNextPosition());

                //if (RaycastHit.transform.CompareTag("enemy"))
                //{
                //    RaycastHit.transform.GetComponent<Enemy>().TakeDamage();
                //}
            }

            if (context.canceled)
            {
                IsLeftClicking = false;
            }
        }

        public void OnDigThrowDirt(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerMovement.IsUsingSpell = true;
                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                PlayerSpells.Spell(0);
                PlayerMovement.IsUsingSpell = false;
            }
        }
        /*
        public void OnTunnelDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerMovement.IsUsingSpell = true;
                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                PlayerSpells.Spell(1);
                PlayerMovement.IsUsingSpell = false;
            }
        }

        public void OnDigDrill(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerMovement.IsUsingSpell = true;
                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                PlayerSpells.Spell(2);
                PlayerMovement.IsUsingSpell = false;
            }
        }
        */
        public void OnDigShockwave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PlayerMovement.IsUsingSpell = true;
                StartCoroutine(CastNextPosition());
            }

            if (context.canceled)
            {
                PlayerSpells.Spell(3);
                PlayerMovement.IsUsingSpell = false;
            }
        }

        #endregion

        #region Utils

        private void FromCameraToWorldPoint()
        {
            RaycastHit value;

            Ray = Player.Camera.ScreenPointToRay(Player.MousePosition);
            Physics.Raycast(Ray, out value);

            RaycastHit = value;

            if (IsLeftClicking == true)
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
            if (RaycastHit.transform != null)
            {
                if (RaycastHit.transform.CompareTag("ground"))
                {
                    PlayerMovement.NextPosition = new Vector3(RaycastHit.point.x, Player.Rigidbody.position.y, RaycastHit.point.z);
                }

                if (RaycastHit.transform.CompareTag("enemy"))
                {
                    PlayerMovement.NextPosition = new Vector3(RaycastHit.transform.position.x, Player.Rigidbody.position.y, RaycastHit.transform.position.z);
                }
            }
        }

        private void SetLookAtMousePosition()
        {
            if (RaycastHit.transform != null)
            {
                if (RaycastHit.transform.CompareTag("ground"))
                {
                    PlayerMovement.LookPosition = new Vector3(RaycastHit.point.x, Player.Rigidbody.position.y, RaycastHit.point.z);
                }

                if (RaycastHit.transform.CompareTag("enemy"))
                {
                    PlayerMovement.LookPosition = new Vector3(RaycastHit.transform.position.x, Player.Rigidbody.position.y, RaycastHit.transform.position.z);
                }
            }
        }

        IEnumerator CastNextPosition()
        {
            while (IsLeftClicking == true || PlayerMovement.IsUsingSpell == true)
            {
                FromCameraToWorldPoint();

                yield return new WaitForFixedUpdate();
            }
        }

        #endregion
    }
}
