using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace caca
{
    public class Chest : MonoBehaviour
    {
        #region Inspector

        public Animator _animator;
        public GameObject _mask;
        public GameObject _areaMask;

        public ItemSpawner _itemSpawner;
        public bool _isTaken = false;
        public bool _isTrapped = false;
        public int _gold;
        public int _nbChest;
        public int _indexChestPack;
        public int _indexEnemyPack;

        public AudioSource audioSource;

        #endregion
    }
}
