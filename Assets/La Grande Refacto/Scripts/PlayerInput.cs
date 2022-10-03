using UnityEngine;
using UnityEngine.InputSystem;

namespace LaGrandeRefacto.Root
{
    public class PlayerInput : MonoBehaviour
    {
        #region Hidden

        private Vector2 _mousePosition;
        private bool _mouseLeftButton;
        private bool _mouseRightButton;

        #endregion


        #region Main

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetMousePosition(context.ReadValue<Vector2>());
            }

            if (context.canceled)
            {

            }
        }

        public void OnMouseLeftButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetMouseLeftButton(true);
            }

            if (context.canceled)
            {
                SetMouseLeftButton(false);
            }
        }

        public void OnMouseRightButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SetMouseRightButton(true);
            }

            if (context.canceled)
            {
                SetMouseRightButton(false);
            }
        }

        #endregion


        #region Getter

        public Vector2 GetMousePosition()
        {
            return _mousePosition;
        }

        public bool GetMouseLeftButton()
        {
            return _mouseLeftButton;
        }

        public bool GetMouseRightButton()
        {
            return _mouseRightButton;
        }

        #endregion


        #region Setter

        private void SetMousePosition(Vector2 value)
        {
            _mousePosition = value;
        }

        private void SetMouseLeftButton(bool value)
        {
            _mouseLeftButton = value;
        }

        private void SetMouseRightButton(bool value)
        {
            _mouseRightButton = value;
        }

        #endregion
    }
}
