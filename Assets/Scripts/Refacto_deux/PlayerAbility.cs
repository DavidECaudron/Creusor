using UnityEngine;

namespace refacto_deux
{
    public class PlayerAbility : MonoBehaviour
    {
        #region Inspector

        [SerializeField] PlayerAbilityScriptableObject _playerAbilityScriptableObject;

        #endregion

        #region Get Set

        public PlayerAbilityScriptableObject PlayerAbilityScriptableObject { get => _playerAbilityScriptableObject; set => _playerAbilityScriptableObject = value; }

        #endregion

        #region Main

        public virtual void UseAbility()
        {
            
        }

        #endregion
    }
}
