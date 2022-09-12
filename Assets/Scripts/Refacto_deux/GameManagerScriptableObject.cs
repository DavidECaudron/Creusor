using UnityEngine;

namespace refacto_deux
{
    [CreateAssetMenu(fileName = "GameManagerScriptableObject", menuName = "ScriptableObjects/GameManagerScriptableObject")]
    public class GameManagerScriptableObject : ScriptableObject
    {
        #region Inspector

        [SerializeField] private float _timeLimitInSeconds;

        #endregion


        #region Hidden

        private GameManager _gameManager;
        private int _gold;

        #endregion


        #region Get Set
 
        public float TimeLimitInSeconds { get => _timeLimitInSeconds; set => _timeLimitInSeconds = value; }
        public GameManager GameManager { get => _gameManager; set => _gameManager = value; }
        public int Gold { get => _gold; set => _gold = value; }

        #endregion
    }
}
