using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    [ExecuteInEditMode]
    public class AbilityRangeRadial : MonoBehaviour
    {
        [Range(1,20)] public float rangeRadius = 1f;
        public CapsuleCollider col;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            col.radius = rangeRadius;
        }
    }
}
