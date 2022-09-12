using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Camera _camera;
        [SerializeField] private GameObject _mask;
        [SerializeField] private GameObject _maskSpawn;
        [SerializeField] private MeshFilter _meshFilter;

        #endregion

        #region Hidden

        private Transform _transform;
        private Rigidbody _rigidbody;

        private Vector2 _mousePosition;

        #endregion

        #region Get Set

        public Camera Camera { get => _camera; private set => _camera = value; }
        public Transform Transform { get => _transform; private set => _transform = value; }
        public Rigidbody Rigidbody { get => _rigidbody; private set => _rigidbody = value; }
        public GameObject Mask { get => _mask; private set => _mask = value; }
        public GameObject MaskSpawn { get => _maskSpawn; private set => _maskSpawn = value; }

        public Vector2 MousePosition { get => _mousePosition; set => _mousePosition = value; }

        #endregion

        #region Updates

        private void Awake()
        {
            Transform = gameObject.GetComponent<Transform>();
            Rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        #endregion

        #region Main
        #endregion

        #region Utils
        #endregion
    }
}
