using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto_deux
{
    public class VegetationRandomizer : MonoBehaviour
    {
        public Gradient gradient;
        Renderer rend; 

        void Awake()
        {
            Color randomColor = gradient.Evaluate(Random.Range(0f, 1f));
            randomColor.a = Random.Range(0f, 1f);
            rend = GetComponent<Renderer>();
            rend.material.SetColor("_RandomColor", randomColor);
        }
    }
}
