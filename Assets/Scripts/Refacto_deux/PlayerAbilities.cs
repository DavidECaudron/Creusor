using System.Collections.Generic;
using UnityEngine;

namespace refacto_deux
{
    public class PlayerAbilities : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PlayerAbilitiesScriptableObject _playerAbilityScriptableObject;
        [SerializeField] private Transform _cloneParent;

        #endregion


        #region Hidden

        private Transform _playerAbilities;

        #endregion


        #region Updates

        private void Awake()
        {
            _playerAbilities = gameObject.GetComponent<Transform>();
            _playerAbilityScriptableObject.CloneParent = _cloneParent;
        }

        private void Start()
        {
            _playerAbilityScriptableObject.AbilityList = new List<PlayerAbility>();

            foreach (Transform item in _playerAbilities)
            {
                _playerAbilityScriptableObject.AbilityList.Add(item.gameObject.GetComponent<PlayerAbility>());
            }
        }

        #endregion
    }
}
