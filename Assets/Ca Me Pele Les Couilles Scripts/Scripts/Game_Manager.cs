using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using proto;
using System.Collections;

namespace caca
{
    public class Game_Manager : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public GameObject _positions;
        public GameObject _player;
        public CurseManager _curseManager;
        public Slider _sliderLeft;
        public Slider _sliderRight;
        public Image _timerImage;
        public Image _normalMonsterImage;
        public Gradient _gradientTimer;
        public Gradient _gradientNormalMonster;
        public CanvasGroup _canvasGroup;
        public TMP_Text _goldText;
        public TMP_Text _chestCounter;
        public Chest_Pack[] _chestPackTable;
        public Enemy_Pack[] _enemyPackTable;

        [Header("Game Manager")]
        public int _limitTime;
        public int _disappearingTime;
        public List<Chest> _listChest = new ();
        public List<Enemy> _listEnemy = new ();
        public float _buffMultiplier;

        [Header("Enemy Pack")]
        public Enemy_Pack _enemyPack;
        public int _nbEnemyPack = 0;

        [Header("Chest Pack")]
        public Chest_Pack _chestPack;
        public int _nbChestPack = 0;

        public AudioSource _audioSourceGameOver;

        public Animator _animatorPostProcessing;
        public CanvasGroup _canvasGroupHUD;
        public CanvasGroup _canvasGroupMap;     
        public CanvasGroup _canvasGroupDialogueFrame;   
        public CanvasGroup _canvasGroupDialogueButton; 

        public TextMeshProUGUI _dialogueText;
        public string[] _introLines;
        //public string[] NoChestLines;
        //public string[] LeaveIslandLines;

        public float _textSpeed = 0.3f;
        int _dialogueIndex = 0;
        int _lineIndex = 0;


        #endregion


        #region Hidden

        private EnemyPosition[] _enemyPackPositions;
        private ChestPosition[] _chestPackPositions;
        private int _gold = 0;
        private int _nbChest = 0;
        private float _startTime;
        private float _tempLimitTime;
        private bool _hasBeenBuff = false;
        private bool _hasBeenCursed = false;

        public bool _introIsEnded = false;
        public bool _inDialogue = false;

        public Animator _pelicanAnimator;

        public AudioSource _pelicanAudioSource;

        public AudioClip[] _pelicanVoiceClips = new AudioClip[4];

        #endregion


        #region Updates

        private void Start()
        {
            ChestSpawn();
            EnemySpawn();
            _canvasGroupHUD.alpha = 0;
            _canvasGroupMap.alpha = 0;
            _canvasGroupDialogueButton.interactable = false;

            _startTime = Time.realtimeSinceStartup;
            _tempLimitTime = (float)_limitTime;
            _gold = PlayerPrefs.GetInt("Gold");
            _goldText.text = _gold.ToString();
            _chestCounter.text = _nbChest.ToString() + "/" + _listChest.Count;
        }

        private void Update()
        {
            if(_introIsEnded)
            {
                TimeManagement();
            }
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

                _enemyPack._numberOfEnemyMelee = _enemyPackPositions[random]._nbEnemyMelee;
                _enemyPack._numberOfEnemyRanged = _enemyPackPositions[random]._nbEnemyRanged;
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

                if (timeLeft <= _tempLimitTime / 4 && _hasBeenCursed == false)
                {
                    _hasBeenCursed = true;
                    _curseManager.TimerAlmostOverFX();
                }
            }
            else
            {
                if (!_player.GetComponent<Player>()._isCursed)
                {
                    _player.GetComponent<Player>()._isCursed = true;
                }

                if (_hasBeenBuff == false)
                {
                    _curseManager.DisplayCurseFX();
                    EnemyBuff();

                    foreach (Enemy item in _listEnemy)
                    {
                        item.Cursed();
                    }

                    for (int i = 0; i < _enemyPackTable.Length; i++)
                    {
                        _enemyPackTable[i].ChasePlayer();
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
                item._healthSlider.fillAmount = item._currentHealth / item._maxHealth;
                item._damageMelee *= _buffMultiplier;
                item._damageRanged *= _buffMultiplier;
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

        public void GameOver()
        {
            StartCoroutine(GameOverTransition());
        }
        IEnumerator GameOverTransition()
        {
            _audioSourceGameOver.Play();
            _animatorPostProcessing.SetBool("GameOver", true);
            _canvasGroupHUD.alpha = 0;
            _canvasGroupMap.alpha = 0;

            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            yield return null;
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

        void NewDialogue()
        {
            _inDialogue = true;
            _dialogueText.text = string.Empty;
            _canvasGroupDialogueFrame.alpha = 1f;
            _pelicanAnimator.SetTrigger("IsTalking");
            StartCoroutine(StartDialogue());
        }

        IEnumerator StartDialogue()
        {
            _pelicanAudioSource.PlayOneShot(_pelicanVoiceClips[Random.Range(0, 3)], 0.3f); 
            foreach (char c in _introLines[_lineIndex].ToCharArray())
            {
                _dialogueText.text += c;
                if(_dialogueText.text.Length == 10)
                {
                    _canvasGroupDialogueButton.interactable = true;
                }
                yield return new WaitForSeconds(_textSpeed);

            }
            yield return null;
        }

        public void DialogueNextButton()
        {
            if(_dialogueText.text == _introLines[_lineIndex])
            {
                if(_lineIndex < _introLines.Length - 1)
                {
                    _lineIndex++;
                    _dialogueText.text = string.Empty;
                    StartCoroutine(StartDialogue());
                }
                else
                {
                    _dialogueText.text = string.Empty;
                    _canvasGroupDialogueFrame.alpha = 0;

                    if(!_introIsEnded)
                    {
                        _introIsEnded = true;
                        _canvasGroupHUD.alpha = 1;
                        _canvasGroupMap.alpha = 1;

                    }
                    _inDialogue = false;
                }
            }
            else
            {
                StopAllCoroutines();
                _dialogueText.text = _introLines[_lineIndex];
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                if(!_introIsEnded)
                {
                    NewDialogue();
                }
                if (_nbChest > 0)
                {

                    //LoadMainMenuAlive();
                }
            }
        }

        #endregion
    }
}
