using UnityEngine;
using UnityEngine.AI;

namespace LaGrandeRefacto.Root
{
    public enum EnemyType
    {
        Melee,
        Ranged
    }

    public class Enemy : MonoBehaviour
    {
        #region Inspector

        [Header("Required")]
        [SerializeField] private Transform _transform;
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [Header("Tweak")]
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private float _speed;

        #endregion


        #region Getter

        public Transform GetTransform()
        {
            return _transform;
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return _navMeshAgent;
        }

        public EnemyType GetEnemyType()
        {
            return _enemyType;
        }

        public float GetSpeed()
        {
            return _speed;
        }

        #endregion
    }
}
