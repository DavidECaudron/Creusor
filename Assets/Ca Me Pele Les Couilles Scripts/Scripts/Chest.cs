using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class Chest : MonoBehaviour
    {
        #region Inspector

        public bool _isTaken = false;
        public bool _isTrapped = false;
        public int _gold;
        public int _nbChest;
        public int _indexChestPack;
        public int _indexEnemyPack;

        #endregion
    }
}
