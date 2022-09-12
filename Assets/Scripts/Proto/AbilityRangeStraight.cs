using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    [ExecuteInEditMode]
    public class AbilityRangeStraight : MonoBehaviour
    {
        [Range(0,10)] public float rangeLenght = 1f;
        [Range(0,20)] public float rangeWith = 1f;
        public Transform range;





        // Start is called before the first frame upda
        void Start()
        {
        }

        void Update()
        {
            range.transform.localScale = new Vector3(rangeWith, 1, rangeLenght);
            
        }
    }
}
