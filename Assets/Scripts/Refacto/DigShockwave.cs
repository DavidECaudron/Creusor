using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class DigShockwave : Ability
    {
        #region Inspector

        [SerializeField] private GameObject _mask;
        [SerializeField] private Transform _maskSpawn;
        [SerializeField] private Transform _maskBin;
        [SerializeField] private MeshFilter _mesh;
        [SerializeField] private AbilityRangeRadius _rangeRadial;

        #endregion

        #region Hidden

        private Collider _collider;

        #endregion

        #region Get Set

        public GameObject Mask { get => _mask; private set => _mask = value; }
        public Transform MaskSpawn { get => _maskSpawn; private set => _maskSpawn = value; }
        public Transform MaskBin { get => _maskBin; private set => _maskBin = value; }
        public MeshFilter Mesh { get => _mesh; private set => _mesh = value; }
        public AbilityRangeRadius RangeRadial { get => _rangeRadial; private set => _rangeRadial = value; }

        #endregion

        #region Updates

        private void Awake()
        {
            RangeRadial = gameObject.GetComponent<AbilityRangeRadius>();
            _collider = RangeRadial.Col;
        }

        private void Start()
        {
            _collider.enabled = false;
        }

        #endregion

        #region Main

        public override void UseAbility()
        {
            base.UseAbility();

            _collider.enabled = true;

            Vector3 position = new Vector3(MaskSpawn.transform.position.x, Mask.transform.position.y, MaskSpawn.transform.position.z);

            GameObject clone = Instantiate(Mask, position, transform.rotation, MaskBin);

            clone.transform.localScale = new Vector3(RangeRadial.RangeRadius, 1.0f, RangeRadial.RangeRadius);

            Mesh.mesh.RecalculateNormals();

            _collider.enabled = false;
        }

        #endregion

        #region Utils
        #endregion
    }
}
