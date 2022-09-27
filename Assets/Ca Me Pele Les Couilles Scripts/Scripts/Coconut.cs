using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class Coconut : MonoBehaviour
    {
        #region Inspector

        public Player player;

        #endregion


        #region Update

        private void Awake()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        #endregion


        #region Util

        private void OnDestroy()
        {
            player._nbCoconut += 1;
        }

        #endregion
    }
}
