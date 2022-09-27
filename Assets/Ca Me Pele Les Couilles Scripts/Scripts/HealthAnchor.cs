using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class HealthAnchor : MonoBehaviour
    {
        public Transform _transform;
        public Transform stats;

        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
        }

        void Update()
        {
            Vector2 anchoredPos = Camera.main.WorldToScreenPoint(_transform.position);
            stats.position = anchoredPos;
        }
    }
}
