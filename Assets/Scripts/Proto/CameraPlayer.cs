using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class CameraPlayer : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private GameObject _player;

        #endregion


        #region Hidden

        private Transform _transform;
        private Transform _playerTransform;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _playerTransform = _player.GetComponent<Transform>();
        }

        private void LateUpdate()
        {
            Vector3 smoothPos = Vector3.Slerp(transform.position, _playerTransform.position, 1.0f);

            _transform.position = new Vector3(smoothPos.x, _transform.position.y, smoothPos.z - 10f);
        }

        #endregion
    }
}
