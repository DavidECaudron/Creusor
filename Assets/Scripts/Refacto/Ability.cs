using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace refacto
{
    public class Ability : MonoBehaviour
    {
        #region Inspector

        [SerializeField] protected float _cooldownInSeconds;
        [SerializeField] protected float _abilitySpeed;
        [SerializeField] protected float _effectDuration;
        [SerializeField] protected int _damage;

        #endregion

        #region Hidden

        private float _timeCheck;

        private bool _isUsable;
        private bool _firstUse;

        #endregion

        #region Get Set

        public float CooldownInSeconds { get => _cooldownInSeconds; set => _cooldownInSeconds = value; }
        public float AbilitySpeed { get => _abilitySpeed; set => _abilitySpeed = value; }
        public float EffectDuration { get => _effectDuration; set => _effectDuration = value; }
        public float TimeCheck { get => _timeCheck; set => _timeCheck = value; }

        public int Damage { get => _damage; set => _damage = value; }

        public bool IsUsable { get => _isUsable; set => _isUsable = value; }
        public bool FirstUse { get => _firstUse; set => _firstUse = value; }


        #endregion

        #region Updates

        private void Start()
        {
            TimeCheck = 0.0f;
            IsUsable = true;
            FirstUse = true;
        }

        #endregion

        #region Main

        public virtual void UseAbility()
        {
            
        }

        #endregion

        #region Utils
        #endregion
    }
}
