using System.Collections.Generic;
using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "PlayerAbilitiesScriptableObject", menuName = "ScriptableObjects/PlayerAbilitiesScriptableObject")]
    public class PlayerAbilitiesScriptableObject : ScriptableObject
    {
        #region Hidden

        private List<PlayerAbility> _abilityList;
        private Transform _cloneParent;

        #endregion


        #region Get Set

        public Transform CloneParent { get => _cloneParent; set => _cloneParent = value; }
        public List<PlayerAbility> AbilityList { get => _abilityList; set => _abilityList = value; }

        #endregion
    }
}
