using UnityEngine;

namespace refacto_deux
{
    public class DigShockwave : PlayerAbility
    {
        #region Inspector

        [SerializeField] private DigShockwaveScriptableObject _digShockwaveScriptableObject;
        [SerializeField] private PlayerAbilitiesScriptableObject _playerAbilitiesScriptableObject;
        [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;

        #endregion


        #region Updates

        private void Awake()
        {
            _digShockwaveScriptableObject.PrefabSpawn = gameObject.GetComponent<Transform>();
        }

        private void Start()
        {
            _digShockwaveScriptableObject.TimeCheck = -_digShockwaveScriptableObject.CooldownInSeconds;
        }

        #endregion


        #region Main

        public override void UseAbility()
        {
            base.UseAbility();

            Vector3 position = new Vector3(_digShockwaveScriptableObject.PrefabSpawn.transform.position.x,
                                           _digShockwaveScriptableObject.Prefab.transform.position.y,
                                           _digShockwaveScriptableObject.PrefabSpawn.transform.position.z);

            _digShockwaveScriptableObject.Clone = Instantiate(_digShockwaveScriptableObject.Prefab, position, transform.rotation, _playerAbilitiesScriptableObject.CloneParent);
            _digShockwaveScriptableObject.TimeCheck = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(position, 4.0f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.transform.CompareTag("chest"))
                {
                    if (hitCollider.transform.parent.GetComponent<Chest>()._isChestTaken == false)
                    {
                        _gameManagerScriptableObject.GameManager.AddGold();
                        hitCollider.transform.parent.GetComponent<Chest>()._isChestTaken = true;
                        Destroy(hitCollider.transform.parent.gameObject, 2.0f);
                    }
                }

                if (hitCollider.transform.CompareTag("enemy"))
                {
                    hitCollider.gameObject.GetComponentInParent<Enemy>().TakeDamage(_digShockwaveScriptableObject.Damage);
                }
            }
        }

        #endregion
    }
}
