using UnityEngine;

namespace refacto_deux
{
    public class DigThrowDirt : PlayerAbility
    {
        #region Inspector

        [SerializeField] private DigThrowDirtScriptableObject _digThrowDirtScriptableObject;
        [SerializeField] private PlayerAbilitiesScriptableObject _playerAbilitiesScriptableObject;
        [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;

        #endregion


        #region Updates

        private void Awake()
        {
            _digThrowDirtScriptableObject.PrefabSpawn = gameObject.GetComponent<Transform>();
        }

        private void Start()
        {
            _digThrowDirtScriptableObject.TimeCheck = -_digThrowDirtScriptableObject.CooldownInSeconds;
        }

        #endregion


        #region Main

        public override void UseAbility()
        {
            base.UseAbility();

            Vector3 position = new Vector3(_digThrowDirtScriptableObject.PrefabSpawn.transform.position.x, 
                                           _digThrowDirtScriptableObject.Prefab.transform.position.y, 
                                           _digThrowDirtScriptableObject.PrefabSpawn.transform.position.z);

            _digThrowDirtScriptableObject.Clone = Instantiate(_digThrowDirtScriptableObject.Prefab, position, transform.rotation, _playerAbilitiesScriptableObject.CloneParent);
            _digThrowDirtScriptableObject.TimeCheck = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(position, 1.0f);
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
                    hitCollider.gameObject.GetComponentInParent<Enemy>().TakeDamage(_digThrowDirtScriptableObject.Damage);
                }
            }
        }

        #endregion
    }
}
