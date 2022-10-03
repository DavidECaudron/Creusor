using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _graphicsTransform;

        [Header("Tweak")]
        [SerializeField] private float _speed;
        [SerializeField] private float _health;

        #endregion


        #region Main

        public void Movement(Vector3 position)
        {
            Vector3 nextPosition = new (position.x, GetTransform().position.y, position.z);
            Vector3 thisPosition = _transform.position;
            Vector3 direction = nextPosition - thisPosition;

            float distance = Vector3.Distance(nextPosition, thisPosition);

            if (distance >= 0.1f)
            {
                _transform.Translate(_speed * Time.deltaTime * direction.normalized);
            }
        }

        public void Rotation(Vector3 position)
        {
            Vector3 lookAtPosition = new (position.x, GetTransform().position.y, position.z);

            GetGraphicsTransform().LookAt(lookAtPosition);
        }

        #endregion


        #region Getter

        public Transform GetTransform()
        {
            return _transform;
        }

        public Transform GetGraphicsTransform()
        {
            return _graphicsTransform;
        }

        #endregion
    }
}
