using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    [ExecuteAlways]
    public class AbilityRangeRadius : MonoBehaviour
    {
        [SerializeField, Range(1, 20)] private float _rangeRadius = 1f;
        [SerializeField] private CapsuleCollider _col;

        public float RangeRadius { get => _rangeRadius; private set => _rangeRadius = value; }
        public CapsuleCollider Col { get => _col; private set => _col = value; }
        
        void Update()
        {
            Col.radius = RangeRadius;
        }
    }
}
