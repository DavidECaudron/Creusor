using System.Collections;
using UnityEngine;

namespace refacto_deux
{
    public class TunnelDash : PlayerAbility
    {
        #region Inspector

        [SerializeField] private GameObject _target;
        [SerializeField] private float _waitTime = 2.0f;
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;

        #endregion

        #region Hidden

        private Transform _targetTransform;

        #endregion

        #region Get Set

        public GameObject Target { get => _target; set => _target = value; }
        public Transform TargetTransform { get => _targetTransform; set => _targetTransform = value; }
        public Transform Transform { get => _transform; set => _transform = value; }
        public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value;}

        #endregion

        #region Updates

        private void Awake()
        {
            TargetTransform = Target.GetComponent<Transform>();
        }

        private void Start()
        {
            //
            //StartCoroutine(MoveUnderground());
        }

        #endregion

        #region Main

        public override void UseAbility()
        {
            
        }

        #endregion

        #region Utils

        IEnumerator MoveUnderground()
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            float elapsedTime = 0.0f;

            Vector3 targetPosition = new Vector3(Transform.position.x, TargetTransform.position.y, Transform.position.z);

            while (elapsedTime < _waitTime)
            {
                transform.position = Vector3.Lerp(Transform.position, targetPosition, (elapsedTime / _waitTime));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            transform.position = targetPosition;

            yield return new WaitForEndOfFrame();
        }

        #endregion
    }
}
