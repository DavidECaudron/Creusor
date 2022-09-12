using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class DigThrowDirt : Ability
    {
        #region Inspector

        [SerializeField] private GameObject _mask;
        [SerializeField] private Transform _maskSpawn;
        [SerializeField] private Transform _maskBin;
        [SerializeField] private MeshFilter _mesh;
        [SerializeField] private float _velocity;
        [SerializeField] private float _angle;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private GameObject _target;

        #endregion

        #region Hidden

        private GameObject _projectileClone;
        private GameObject _maskClone;

        #endregion

        #region Get Set

        public GameObject Mask { get => _mask; set => _mask = value; }
        public Transform MaskSpawn { get => _maskSpawn; set => _maskSpawn = value; }
        public Transform MaskBin { get => _maskBin; set => _maskBin = value; }
        public MeshFilter Mesh { get => _mesh; set => _mesh = value; }

        #endregion

        #region Updates
        #endregion

        #region Main

        public override void UseAbility()
        {
            base.UseAbility();

            Vector3 position = new Vector3(MaskSpawn.transform.position.x, Mask.transform.position.y, MaskSpawn.transform.position.z);

            _maskClone = Instantiate(Mask, position, transform.rotation, MaskBin);
            _projectileClone = Instantiate(_projectile, position, transform.rotation, MaskBin);

            Mesh.mesh.RecalculateNormals();

            float angle = _angle * Mathf.Deg2Rad;

            StartCoroutine(Projectile(_velocity, angle));
        }

        #endregion

        #region Utils

        IEnumerator Projectile(float velocity, float angle)
        {
            float time = 0.0f;
            Vector3 direction = _target.transform.position - _maskClone.transform.position;
            Vector3 groundDirection = new Vector3(direction.x, 0.0f, direction.z);

            while (time < 100)
            {
                float x = velocity * time * Mathf.Cos(angle);
                float y = velocity * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);

                _projectileClone.transform.rotation = _maskClone.transform.rotation;
                _projectileClone.transform.position = _maskClone.transform.position + groundDirection.normalized * x + Vector3.up * y; 

                time += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }   
        }

        #endregion
    }
}
