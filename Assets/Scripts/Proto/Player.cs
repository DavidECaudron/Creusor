using UnityEngine;
using UnityEngine.InputSystem;

namespace proto
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private float _moveSpeed = 5.0f;
        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _mask;
        [SerializeField] private GameObject _maskSpawn;
        //[SerializeField] private GameObject _meshFilter;

        #endregion


        #region Hidden

        private Transform _transform;
        private Rigidbody _rigidBody;

        private Vector2 _mousePosition = new Vector2(0.0f, 0.0f);
        private Vector3 _targetPosition = new Vector3(0.0f, 0.0f, 0.0f);

        private bool _isLeftClicking = false;

        private Ray _screenToRay;
        private RaycastHit _screenToWorld;

        //private Mesh _mesh;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _rigidBody = gameObject.GetComponent<Rigidbody>();
            _targetPosition = _rigidBody.position;

            //_mesh = _meshFilter.GetComponent<MeshFilter>().mesh;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        #endregion


        #region Input System

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();

            if (_isLeftClicking)
            {
                _screenToRay = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_screenToRay, out _screenToWorld);
            }
        }

        public void OnMouseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isLeftClicking = true;

                _screenToRay = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_screenToRay, out _screenToWorld);

                if (_screenToWorld.transform.CompareTag("enemy"))
                {
                    _screenToWorld.transform.GetComponent<Enemy>().TakeDamage();
                }
            }

            if (context.canceled)
            {
                _isLeftClicking = false;

                _screenToRay = _camera.ScreenPointToRay(_mousePosition);
                Physics.Raycast(_screenToRay, out _screenToWorld);
            }
        }

        public void OnSpellOne(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Instantiate(_mask, new Vector3(_maskSpawn.transform.position.x, _mask.transform.position.y, _maskSpawn.transform.position.z), _transform.rotation);
                //_mesh.RecalculateNormals();
            }

            if (context.canceled)
            {

            }
        }

        #endregion


        #region Main

        private void Movement()
        {
            if (_screenToWorld.transform != null)
            {
                if (_screenToWorld.transform.CompareTag("ground"))
                {
                    _targetPosition = new Vector3(_screenToWorld.point.x, _rigidBody.position.y, _screenToWorld.point.z);
                }

                if (_screenToWorld.transform.CompareTag("enemy"))
                {
                    _targetPosition = new Vector3(_screenToWorld.transform.position.x, _rigidBody.position.y, _screenToWorld.transform.position.z);
                }
            }

            _transform.LookAt(new Vector3(_targetPosition.x, _rigidBody.position.y, _targetPosition.z));

            float distance = Vector3.Distance(new Vector3(_targetPosition.x, _rigidBody.position.y, _targetPosition.z), new Vector3(_rigidBody.position.x, _rigidBody.position.y, _rigidBody.position.z));

            Vector3 vector = _targetPosition - new Vector3(_rigidBody.position.x, _rigidBody.position.y, _rigidBody.position.z);

            if (distance < 2.0f) return;

            if (_rigidBody.velocity.magnitude < _moveSpeed / 2)
            {
                _rigidBody.AddForce(vector.normalized * _moveSpeed * 2 * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }

        #endregion


        #region Utils

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Enemy>().IsInRange = true;

            if (_rigidBody.velocity.magnitude != 0)
            {
                other.GetComponent<Enemy>().TakeDamage();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            other.GetComponent<Enemy>().IsInRange = false;
        }

        #endregion
    }
}
