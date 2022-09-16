using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace caca
{
    public class Game_Manager : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Slider _sliderLeft;
        public Slider _sliderRight;
        public Image _timerImage;
        public Image _eyeImage;
        public Gradient _gradientTimer;
        public Gradient _gradientEye;
        public CanvasGroup _canvasGroup;
        public TMP_Text _goldText;
        public TMP_Text _chestCounter;

        [Header("Game Manager")]
        public int _limitTime;
        public int _disappearingTime;
        public List<Chest> _listChest = new List<Chest>();
        public List<Enemy> _listEnemy = new List<Enemy>();
        public float _buffMultiplier;

        #endregion


        #region Hidden

        private int _gold = 0;
        private int _nbChest = 0;
        private float _startTime;
        private float _tempLimitTime;
        private float _tempDisappearingTime;
        private bool _hasBeenBuff = false;

        #endregion


        #region Updates

        private void Start()
        {
            _startTime = Time.realtimeSinceStartup;
            _tempLimitTime = (float)_limitTime;
            _tempDisappearingTime = (float)_disappearingTime;
            _gold = PlayerPrefs.GetInt("Gold");
            _goldText.text = _gold.ToString();
            _chestCounter.text = _nbChest.ToString() + " / 3";
        }

        private void Update()
        {
            TimeManagement();
        }

        #endregion


        #region main

        public void TimeManagement()
        {
            float seconds = Time.realtimeSinceStartup - _startTime;
            float timeLeft = _tempLimitTime - seconds;

            if (timeLeft > 0)
            {
                _sliderLeft.value = 1 - (timeLeft / _tempLimitTime);
                _sliderRight.value = 1 - (timeLeft / _tempLimitTime);

                _timerImage.color = _gradientTimer.Evaluate(1 - (timeLeft / _tempLimitTime));
                _eyeImage.color = _gradientEye.Evaluate(1 - (timeLeft / _tempLimitTime));
            }
            else
            {
                float _alpha = _tempDisappearingTime + _tempLimitTime - seconds;

                if (_alpha > 0)
                {
                    _canvasGroup.alpha = (_alpha / _tempDisappearingTime);
                }
                else
                {
                    if (_hasBeenBuff == false)
                    {
                        EnemyBuff();
                    }
                }
            }
        }

        public void AddGold(int gold)
        {
            _gold += gold;
            _goldText.text = _gold.ToString();
        }

        public void AddChest(int chest)
        {
            _nbChest += chest;
            _chestCounter.text = _nbChest.ToString() + " / 3";
        }

        public void EnemyBuff()
        {
            foreach (Enemy item in _listEnemy)
            {
                if (item._isAlive == false)
                {
                    item._isAlive = true;
                    item._transform.position = item._initialPosition;
                    item._navMeshAgent.enabled = true;
                }

                item._maxHealth *= _buffMultiplier;
                item._currentHealth = item._maxHealth;
                item._healthSlider.value = item._currentHealth / item._maxHealth;
                item._damage *= _buffMultiplier;
                item._movementSpeed *= _buffMultiplier;

                _hasBeenBuff = true;
            }
        }

        #endregion


        #region Utils

        public void LoadMainMenu()
        {
            PlayerPrefs.SetInt("Gold", _gold);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void AddEnemyInTable(Enemy enemy)
        {
            _listEnemy.Add(enemy);
        }

        public void RemoveEnemyFromTable(Enemy enemy)
        {
            _listEnemy.Remove(enemy);
        }

        public void AddChestInTable(Chest chest)
        {
            _listChest.Add(chest);
        }

        public void RemoveChestFromTable(Chest chest)
        {
            _listChest.Remove(chest);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                if (_nbChest > 0)
                {
                    LoadMainMenu();
                }
            }
        }

        #endregion
    }
}
