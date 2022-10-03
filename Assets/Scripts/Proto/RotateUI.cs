using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class RotateUI : MonoBehaviour
    {
        public float rotationSpeed = 4f;

        void Update()
        {
            transform.Rotate(0,0,(rotationSpeed * 100) * Time.deltaTime);
        }
    }
}
