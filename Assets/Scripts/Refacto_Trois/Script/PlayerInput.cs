using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Refacto_Trois
{
    public class PlayerInput : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Player_Data _playerData;

        #endregion


        #region Hidden

        private Vector2 _mousePosition = new Vector2(0, 0);
        private Ray _ray;
        private RaycastHit _hit;

        private bool _isLeftClicking = false;

        #endregion


        #region Main

        public void OnMouseButton(InputAction.CallbackContext context)
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

        public void OnDigThrowDirt(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                
            }

            if (context.canceled)
            {
                
            }
        }

        public void OnTunnelDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                
            }

            if (context.canceled)
            {
                
            }
        }

        public void OnDigShockwave(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
               
            }

            if (context.canceled)
            {
                
            }
        }

        #endregion


        #region Utils

        IEnumerator CastNextPosition()
        {
            while (_isLeftClicking == true)
            {
                FromCameraToWorldPoint();

                yield return new WaitForEndOfFrame();
            }
        }

        private void FromCameraToWorldPoint()
        {
            RaycastHit value;

            _ray = _playerData.PlayerCamera.Camera.ScreenPointToRay(_mousePosition);
            Physics.Raycast(_ray, out value);

            _hit = value;

            Transform test = _hit.transform;

            if (test != null)
            {
                if (test.CompareTag("ground"))
                {
                    _playerData.PlayerMovement.SetNextPosition(_hit.point);
                }
            }
        }

        #endregion
    }
}
