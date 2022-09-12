using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace refacto_deux
{
    public class GameManager : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private GameManagerScriptableObject _gameManagerScriptableObject;
        [SerializeField] private TMP_Text _textGold;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private GameObject _prefabEnemy;
        [SerializeField] private GameObject _player;

        #endregion


        #region Hidden

        private bool _isTimerRunning = false;
        private float _startTime;
        private int _timeTemp;
        private int _nbWave;

        #endregion


        #region Updates

        private void Awake()
        {
            _gameManagerScriptableObject.GameManager = this;
            _textGold.text = _gameManagerScriptableObject.Gold.ToString();
        }

        private void Start()
        {
            _gameManagerScriptableObject.Gold = 0;
            _textGold.text = _gameManagerScriptableObject.Gold.ToString();
            _isTimerRunning = true;
            _startTime = Time.time;
            _timeTemp = 9999;
            _nbWave = 0;
        }

        private void Update()
        {
            TimerClock();
        }

        #endregion


        #region Main

        private void TimerClock()
        {
            if (_isTimerRunning == true)
            {
                float _temp = _gameManagerScriptableObject.TimeLimitInSeconds - Time.time + _startTime;

                string _minutes = (Mathf.RoundToInt(_temp) / 60).ToString("00");
                string _seconds = (Mathf.RoundToInt(_temp) % 60).ToString("00");

                if (Mathf.RoundToInt(_temp) < _timeTemp)
                {
                    _textTimer.text = _minutes + ":" + _seconds;

                    if (Mathf.RoundToInt(_temp) % 30 == 0)
                    {
                        foreach (Transform item in _prefabEnemy.transform)
                        {
                            Enemy enemy = item.GetComponent<Enemy>();
                            enemy._playerGameObject = _player;
                            enemy._maxHealth += 25 * _nbWave;
                            enemy.enabled = true;
                        }

                        Instantiate(_prefabEnemy, _prefabEnemy.transform.position, _prefabEnemy.transform.rotation, gameObject.transform);

                        _nbWave += 1;

                        _timeTemp = Mathf.RoundToInt(_temp);
                    }

                    if (_temp <= 0)
                    {
                        _isTimerRunning = false;

                        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                    }
                }
            }
        }

        public void AddGold()
        {
            _gameManagerScriptableObject.Gold += 25;
            _textGold.text = _gameManagerScriptableObject.Gold.ToString();
        }

        #endregion
    }
}
