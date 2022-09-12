using UnityEngine;

namespace refacto_deux
{
    public class CameraPlayer : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PlayerScriptableObject _playerScriptableObject;
        [SerializeField] private PlayerInputScriptableObject _playerInputScriptableObject;

        #endregion


        #region Hidden

        private Transform _transform;

        #endregion


        #region Updates

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
            _playerInputScriptableObject.Camera = gameObject.GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            Vector3 smoothPos = Vector3.Slerp(transform.position, 
                                              _playerScriptableObject.PlayerTransform.position, 
                                              1.0f);

            _transform.position = new Vector3(smoothPos.x, _transform.position.y, smoothPos.z - 10f);
        }

        #endregion
    }
}
