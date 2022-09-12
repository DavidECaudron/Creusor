using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace refacto_deux
{
    public class Player : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private PlayerScriptableObject _playerScriptableObject;
        [SerializeField] private int _maxHealth;
        [SerializeField] private Image _healthBar;

        #endregion


        #region Hidden

        private int _currentHealth;

        #endregion


        #region Updates

        private void Awake()
        {
            _playerScriptableObject.PlayerTransform = gameObject.GetComponent<Transform>();
            _playerScriptableObject.PlayerRigidbody = gameObject.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        #endregion


        #region Main

        public void TakeDamage(int damage)
        {
            if (_currentHealth - damage > 0)
            {
                _currentHealth -= damage;
                _healthBar.fillAmount = (float)_currentHealth / _maxHealth;
            }
            else
            {
                //Destroy(gameObject);
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
        }

        #endregion
    }
}
