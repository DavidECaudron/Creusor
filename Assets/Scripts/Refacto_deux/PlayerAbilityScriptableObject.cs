using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "PlayerAbilityScriptableObject", menuName = "ScriptableObjects/PlayerAbilityScriptableObject")]
    public class PlayerAbilityScriptableObject : ScriptableObject
    {
        #region Inspector

        [SerializeField] protected GameObject _prefab;
        
        [SerializeField] protected float _cooldownInSeconds;
        [SerializeField] protected int _damage;

        #endregion


        #region Hidden

        protected GameObject _clone;
        protected Transform _prefabSpawn;
        protected float _timeCheck;
        
        #endregion


        #region Get Set

        public GameObject Prefab { get => _prefab; set => _prefab = value;}
        public Transform PrefabSpawn { get => _prefabSpawn; set => _prefabSpawn = value;}
        public GameObject Clone { get => _clone; set => _clone = value;}

        public float CooldownInSeconds { get => _cooldownInSeconds; set => _cooldownInSeconds = value; }
        public int Damage { get => _damage; set => _damage = value; }
        public float TimeCheck { get => _timeCheck; set => _timeCheck = value; }

        #endregion
    }
}
