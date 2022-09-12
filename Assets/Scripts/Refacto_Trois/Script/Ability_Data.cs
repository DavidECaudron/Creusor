using UnityEngine;

namespace Refacto_Trois
{
    [CreateAssetMenu(fileName = "Ability_Data", menuName = "Data/Ability_Data")]
    public class Ability_Data : ScriptableObject
    {
        #region Inspector

        [Header("Script")]
        [SerializeField] private Ability _ability;
        [SerializeField] private AbilityDig _abilityDig;
        [SerializeField] private AbilityShockwave _abilityShockwave;
        [SerializeField] private AbilityTunnel _abilityTunnel;

        [Header("Data")]
        [SerializeField] private GameManager_Data _gameManagerData;

        [Header("Tweak Dig")]
        [SerializeField] private int _cooldown_Dig;
        [SerializeField] private int _damage_Dig;

        [Header("Tweak Tunnel")]
        [SerializeField] private int _cooldown_Tunnel;
        [SerializeField] private int _damage_Tunnel;

        [Header("Tweak Shockwave")]
        [SerializeField] private int _cooldown_Shockwave;
        [SerializeField] private int _damage_Shockwave;

        #endregion


        #region Get Set

        public Ability Ability { get => _ability; private set => _ability = value; }
        public AbilityDig AbilityDig { get => _abilityDig; private set => _abilityDig = value; }
        public AbilityShockwave AbilityShockwave { get => _abilityShockwave; private set => _abilityShockwave = value; }
        public AbilityTunnel AbilityTunnel { get => _abilityTunnel; private set => _abilityTunnel = value; }
        public GameManager_Data GameManagerData { get => _gameManagerData; private set => _gameManagerData = value; }

        #endregion
    }
}
