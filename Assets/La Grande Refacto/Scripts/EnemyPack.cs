using System.Collections.Generic;
using UnityEngine;

namespace LaGrandeRefacto.Root
{
    public class EnemyPack : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private GameObject _enemy;
        [SerializeField] private Enemy _enemyScript;

        [Header("Tweak")]
        [SerializeField] private int _nbMelee;
        [SerializeField] private int _nbRanged;

        [Header("Debug")]
        [SerializeField] private List<Enemy> _enemyList;
        [SerializeField] private Vector3 _initialPosition;

        #endregion


        #region Getter

        public GameObject GetEnemy()
        {
            return _enemy;
        }

        public Enemy GetEnemyScript()
        {
            return _enemyScript;
        }

        public int GetNbMelee()
        {
            return _nbMelee;
        }

        public int GetNbRanged()
        {
            return _nbRanged;
        }

        public List<Enemy> GetEnemyList()
        {
            return _enemyList;
        }

        #endregion
    }
}
