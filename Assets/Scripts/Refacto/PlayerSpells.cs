using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class PlayerSpells : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Transform _maskBin;
        [SerializeField] private Transform _abilities;

        #endregion

        #region Hidden

        private List<Ability> _abilityList;

        #endregion

        #region Get Set

        public Transform MaskBin { get => _maskBin; private set => _maskBin = value; }
        public List<Ability> AbilityList { get => _abilityList; private set => _abilityList = value;}

        #endregion

        #region Updates

        private void Start()
        {
            AbilityList = new List<Ability>();

            foreach (Transform item in _abilities)
            {
                AbilityList.Add(item.gameObject.GetComponent<Ability>());
            }
        }

        #endregion

        #region Main

        public void Spell(int num)
        {
            if (Time.time >= AbilityList[num].TimeCheck + AbilityList[num].CooldownInSeconds)
            {
                UseSpell(num);
            }
            else
            {
                if (AbilityList[num].FirstUse == true)
                {
                    UseSpell(num);
                    AbilityList[num].FirstUse = false;
                }

                return;
            }
        }

        #endregion

        #region Utils

        private void UseSpell(int num)
        {
            AbilityList[num].TimeCheck = Time.time;
            AbilityList[num].UseAbility();
        }

        #endregion
    }
}
