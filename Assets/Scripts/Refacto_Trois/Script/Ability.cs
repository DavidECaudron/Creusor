using UnityEngine;

namespace Refacto_Trois
{
    public class Ability : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Ability_Data _abilityData;
        [SerializeField] private Transform _bin;

        #endregion


        #region Hidden

        private Transform _abilityTransform;
        private AbilityDig _dig;
        private AbilityTunnel _tunnel;
        private AbilityShockwave _shockwave;

        #endregion


        #region Updates

        private void Awake()
        {
            _abilityTransform = gameObject.GetComponent<Transform>();
            _dig = gameObject.GetComponentInChildren<AbilityDig>();
            _tunnel = gameObject.GetComponentInChildren<AbilityTunnel>();
            _shockwave = gameObject.GetComponentInChildren<AbilityShockwave>();
        }

        #endregion
    }
}
