using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class EnemyProjectile : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private Transform _transform;

        [Header("tweak")]
        [SerializeField] private float _speed;
        [SerializeField] private float _timerDestroy;

        #endregion


        #region Updates

        private void Start()
        {
            Destroy(gameObject, _timerDestroy);
        }

        private void Update()
        {
            _transform.Translate(_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
        }

        #endregion
    }
}
