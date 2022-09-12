using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class DigDrill : Ability
    {
        #region Inspector

        [SerializeField] private GameObject _mask;
        [SerializeField] private Transform _maskSpawn;
        [SerializeField] private Transform _maskBin;
        [SerializeField] private MeshFilter _mesh;

        #endregion

        #region Hidden
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

            Instantiate(Mask, position, transform.rotation, MaskBin);

            Mesh.mesh.RecalculateNormals();
        }

        #endregion

        #region Utils
        #endregion
    }
}
