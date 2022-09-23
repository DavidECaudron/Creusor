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
        public GameObject _positions;
        public GameObject _player;
        public Slider _sliderLeft;
        public Slider _sliderRight;
        public Image _timerImage;
        public Image _eyeImage;
        public Gradient _gradientTimer;
        public Gradient _gradientEye;
        public CanvasGroup _canvasGroup;
        public TMP_Text _goldText;
        public TMP_Text _chestCounter;
        public Chest_Pack[] _chestPackTable;
        public Enemy_Pack[] _enemyPackTable;

        [Header("Game Manager")]
        public int _limitTime;
        public int _disappearingTime;
        public List<Chest> _listChest = new List<Chest>();
        public List<Enemy> _listEnemy = new List<Enemy>();
        public float _buffMultiplier;

        [Header("Enemy Pack")]
        public Enemy_Pack _enemyPack;
        public int _nbEnemyPack = 0;

        [Header("Chest Pack")]
        public Chest_Pack _chestPack;
        public int _nbChestPack = 0;

        #endregion


        #region Hidden

        private EnemyPosition[] _enemyPackPositions;
        private ChestPosition[] _chestPackPositions;
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
            ChestSpawn();
            EnemySpawn();

            _startTime = Time.realtimeSinceStartup;
            _tempLimitTime = (float)_limitTime;
            _tempDisappearingTime = (float)_disappearingTime;
            _gold = PlayerPrefs.GetInt("Gold");
            _goldText.text = _gold.ToString();
            _chestCounter.text = _nbChest.ToString() + "/" + _listChest.Count;
        }

        private void Update()
        {
            TimeManagement();
        }

        #endregion


        #region main

        public void ChestSpawn()
        {
            _chestPack._gameManager = this;

            int index = 0;

            _chestPackPositions = new ChestPosition[_positions.transform.childCount];
            _chestPackTable = new Chest_Pack[_nbChestPack];

            foreach (Transform item in _positions.transform)
            {
                _chestPackPositions[index] = item.gameObject.GetComponent<ChestPosition>();

                index += 1;
            }

            for (int i = 0; i < _nbChestPack; i++)
            {
                int random = Random.Range(0, _chestPackPositions.Length);

                _chestPack._numberOfChest = _chestPackPositions[random]._nbChest;
                _chestPack._spawnAreaRange = _chestPackPositions[random]._spawnAreaRange;

                if (_chestPackPositions[random]._hasBeenUsed == false)
                {
                    Chest_Pack clone = Instantiate(_chestPack, _chestPackPositions[random]._transform.position, Quaternion.identity, this.transform);

                    _chestPackPositions[random]._index = i;

                    _chestPackTable[i] = clone;

                    _chestPackPositions[random]._hasBeenUsed = true;
                }
                else
                {
                    i -= 1;
                }
            }
        }

        public void EnemySpawn()
        {
            _enemyPack._gameManager = this;
            _enemyPack._playerTransform = _player.GetComponent<Transform>();
            _enemyPack._cameraTransform = _player.GetComponent<Player>()._camera.transform;

            int index = 0;

            _enemyPackPositions = new EnemyPosition[_positions.transform.childCount];
            _enemyPackTable = new Enemy_Pack[_nbEnemyPack];

            foreach (Transform item in _positions.transform)
            {
                _enemyPackPositions[index] = item.gameObject.GetComponent<EnemyPosition>();

                index += 1;
            }

            for (int i = 0; i < _nbEnemyPack; i++)
            {
                int random = Random.Range(0, _enemyPackPositions.Length);

                _enemyPack._numberOfEnemy = _enemyPackPositions[random]._nbEnemy;
                _enemyPack._spawnAreaRange = _enemyPackPositions[random]._spawnAreaRange;

                if (_enemyPackPositions[random]._hasBeenUsed == false)
                {
                    Enemy_Pack clone = Instantiate(_enemyPack, _enemyPackPositions[random]._transform.position, Quaternion.identity, this.transform);

                    _enemyPackTable[i] = clone;

                    ChestPosition chestPosTemp = _enemyPackPositions[random].gameObject.GetComponent<ChestPosition>();

                    if (chestPosTemp != null && chestPosTemp._hasBeenUsed == true)
                    {
                        clone.HideEnemy();

                        _chestPackTable[chestPosTemp._index].TrapChest(chestPosTemp._index, i);
                    }

                    _enemyPackPositions[random]._hasBeenUsed = true;
                }
                else
                {
                    i -= 1;
                }
            }
        }

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
                    item.ShowEnemy();
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

        public void LoadMainMenuAlive()
        {
            PlayerPrefs.SetInt("Gold", _gold);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        public void LoadMainMenuDead()
        {
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
                    LoadMainMenuAlive();
                }
            }
        }

        #endregion
    }
}
