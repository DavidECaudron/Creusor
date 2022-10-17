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
        public AudioSource _audioSourceIslandLoop;
        public AudioSource _audioSourceCurseLoop;
        public AudioSource _audioSourceGold;
        public AudioClip _goldPickupClip;
        public Animator _animatorPostProcessing;
        public Animator _sceneTransitionAnimator;
        public Animator _goldCountAnimator; 
        public Animator _chestIconAnimator;

        public CanvasGroup _canvasGroupHUD;
        public CanvasGroup _canvasGroupMap;     

        public CanvasGroup _canvasGroupCurse;
        public CanvasGroup _canvasGroupDialogueFrame;   
        public CanvasGroup _canvasGroupDialogueButton; 
        public CanvasGroup _canvasGroupDialogueChoices; 
        public CanvasGroup _canvasGroupPelican; 
        public CanvasGroup[] _pelicanEmotionPictures = new CanvasGroup[2]; 

        public TextMeshProUGUI _dialogueText;
        string[] _lines = new string[9];
        int[] _lineEmotionIndexes = new int[9];

        public float _textSpeed = 0.3f;
        public float _introTimeDifference;
        int _currentLineCount = 0;
        int _lineIndex = 0;
        int _dialogueStep = 0;
        int[] _linePerDialogue = new int[2];
        public bool _introIsEnded = false;
        public bool _inDialogue = false;

        public SphereCollider _dialogueCollider;

        public bool _nearOfPelican = false;

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

        public bool _gameRunEnded = false;


        public Animator _pelicanAnimator;

        public AudioSource _pelicanAudioSource;

        public AudioClip[] _pelicanVoiceClips = new AudioClip[5];
        public int _pelicanVoiceIndex = 0;

        #endregion


        #region Updates

        private void Start()
        {   
            Invoke("DisplayScene", 2f);  
            //Step 1
            _linePerDialogue[0] = 5;
            _lines[0] = "Ohé camarade ! Es-tu prêt à te lancer dans cette quête de richesse au péril de ta vie ?";
            _lineEmotionIndexes[0] = 0;
            _lines[1] = "Si tu veux continuer à voyager d'île en île, il va faloir payer grassement l'incroyable navigateur que je suis !";
            _lineEmotionIndexes[1] = 0;
            _lines[2] = "Je te conseil de faire attention à la malédiction de l'île, ça enrage les monstres. Tu ne ferais pas long feu. Crois-moi.";
            _lineEmotionIndexes[2] = 0;  
            _lines[3] = "Parcontre, inutile de te dire que revenir bredouille n'est pas une option ! Utilise ta boussole pour trouver où creuser !";
            _lineEmotionIndexes[3] = 1;
            _lines[4] = "On ne perd pas de temps, À vos marques... prêt... CREUSEZ !!!";
            _lineEmotionIndexes[4] = 0;

            //Step - 02        
            _linePerDialogue[1] = 3;
            _lines[5] = "Tu es de retour !? Regardons un peu ce que tu nous rapporte de cette chasse aux trésors...";
            _lineEmotionIndexes[5] = 0; 
            // _lines[6] is variable in coroutine //    
            // _lines[7] is variable in coroutine //           
            _lines[8] = "On mets les voiles ? On ne pourra probablement plus revenir sur cette île après !";
            _lineEmotionIndexes[8] = 0;     

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

        void DisplayScene()
        {
            _sceneTransitionAnimator.SetBool("HideScreen", false);
        }
        void HideScene()
        {
            _sceneTransitionAnimator.SetBool("HideScreen", true);
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
            float seconds = Time.realtimeSinceStartup - _introTimeDifference;
            float timeLeft = _tempLimitTime - seconds;

            if (timeLeft > 0)
            {
                _sliderLeft.value = 1 - (timeLeft / _tempLimitTime);
                _sliderRight.value = 1 - (timeLeft / _tempLimitTime);

                if (timeLeft <= _tempLimitTime / 7 && _hasBeenCursed == false)
                {
                    _hasBeenCursed = true;
                    _curseManager.TimerAlmostOverFX();
                    _lines[5] = "Tu as reçu un coffre sur la tête !? La malédiction de l'île est réveillée, On doit s'en aller !";
                    _lineEmotionIndexes[6] = 1;
                }
            }
            else
            {
                if (!_player.GetComponent<Player>()._isCursed)
                {
                    _player.GetComponent<Player>()._isCursed = true;
                    _player.GetComponent<Player>().CurseConsomable();
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

        public void AddGold(int addedGold)
        {
            StartCoroutine(UpdadeGoldCount(addedGold, _gold));   
        }

        IEnumerator UpdadeGoldCount(int addedGold, int previousGoldSold)
        {
            for(int i = 0; i < addedGold; i++)
            {
                _gold += 1;
                _goldText.text = _gold.ToString("0000");
                _goldCountAnimator.SetTrigger("CollectGold");
                if(i%3==0)
                {
                    _audioSourceGold.PlayOneShot(_goldPickupClip);
                }
                yield return new WaitForSeconds(0.001f);
                yield return null;
            }

            _gold = previousGoldSold + addedGold;
            _goldText.text = _gold.ToString("0000");
            
            yield return null;
        }

        public void AddChest(int chest)
        {
            _nbChest += chest;
            _chestCounter.text = _nbChest.ToString() + " / 3";
            _chestIconAnimator.SetTrigger("CollectChest");
            _linePerDialogue[1] = 4; // Unlock Island leaving;
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
            _gameRunEnded = true;
            StartCoroutine(GameOverTransition());
        }
        IEnumerator GameOverTransition()
        {
            _audioSourceGameOver.Play();
            _animatorPostProcessing.SetBool("GameOver", true);
            _canvasGroupHUD.alpha = 0;
            _canvasGroupMap.alpha = 0;
            _canvasGroupCurse.alpha = 0;            

            yield return new WaitForSeconds(3f);
            HideScene();  
            yield return new WaitForSeconds(2f);
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

        public void NewDialogue()
        {
            _inDialogue = true;
            _dialogueText.text = string.Empty;
            _canvasGroupDialogueFrame.alpha = 1f;
            _canvasGroupHUD.alpha = 0;
            _canvasGroupMap.alpha = 0;
            _canvasGroupPelican.alpha = 0;
            StartCoroutine(StartDialogue());
        }

        IEnumerator StartDialogue()
        {
            if(_lineEmotionIndexes[_lineIndex] == 0)
            {
                _pelicanAudioSource.PlayOneShot(_pelicanVoiceClips[_pelicanVoiceIndex], 0.3f);
                _pelicanAnimator.SetTrigger("IsTalking");
                _pelicanVoiceIndex++;

                if(_pelicanVoiceIndex > 2)
                {
                    _pelicanVoiceIndex = 0;
                }
                _pelicanEmotionPictures[0].alpha = 1;
                _pelicanEmotionPictures[1].alpha = 0;

            }
            else if(_lineEmotionIndexes[_lineIndex] == 1)
            {
                _pelicanAudioSource.PlayOneShot(_pelicanVoiceClips[3], 0.3f);
                _pelicanEmotionPictures[0].alpha = 0;
                _pelicanEmotionPictures[1].alpha = 1;
            }
            else if(_lineEmotionIndexes[_lineIndex] == 1)
            {
                _pelicanAudioSource.PlayOneShot(_pelicanVoiceClips[4], 0.3f);
                _pelicanEmotionPictures[0].alpha = 1;
                _pelicanEmotionPictures[1].alpha = 0;
            }

            if(_dialogueStep == 1)
            {
                if(_nbChest == 0)
                {
                    _lines[6] = "Comment !? Tu oses revenir bredouille ! Je pensais que nôtre accord était pourtant simple :";
                    _lineEmotionIndexes[6] = 1;
                    _lines[7] = "Je prend une partie du butin, Mais si tu ne ramènes rien je ne t’embarque pas ! Alors on se bouge !";
                    _lineEmotionIndexes[7] = 1;
                }
                else if(_nbChest == 1)
                {
                    _lines[6] = "C'est un maigre début, mais je suppose que je vais devoir faire avec cette fois... ";
                    _lineEmotionIndexes[6] = 1;
                    _lines[7] = "j'ai placé de meilleurs espoirs en toi le bleu ! Rapportes-moi plus de trésors la prochaine fois !";    
                    _lineEmotionIndexes[7] = 0;    
                }
                else if(_nbChest == 2)
                {   
                    _lines[6] = "Par mon bec ! C'est un beau butin que tu ramènes là mon garçon ! Tu as un bon coup de pelle !";
                    _lineEmotionIndexes[6] = 2;        
                    _lines[7] = "Je suis certain que ta prochaine chasse sera encore plus profitable héhé !";                    
                    _lineEmotionIndexes[7] = 0;  
                
                }
                else if(_nbChest == 3)
                {
                    _lines[6] = "Yo-ho-ho ! Je dois reconnaître qu'aucun pirate ne manie la pelle aussi bien que toi !";
                    _lineEmotionIndexes[6] = 2;  
                    _lines[7] = "Par mes yeux ! Tant de richesse pour moi... Heuuu je veux dire nous !";
                    _lineEmotionIndexes[7] = 2;  
                }
            }

            foreach (char c in _lines[_lineIndex].ToCharArray())
            {
                _dialogueText.text += c;
                if(_dialogueText.text.Length == 10 && _lineIndex < 8)
                {
                    _canvasGroupDialogueButton.interactable = true;
                }
                yield return new WaitForSeconds(_textSpeed);

            }
            yield return null;

            if(_lineIndex == 8)
            {
                _canvasGroupDialogueChoices.interactable = true;
                _canvasGroupDialogueChoices.alpha = 1;
            }
        }

        public void DialogueNextButton()
        {
            if(_dialogueText.text == _lines[_lineIndex])
            {
                if(_lines[_lineIndex] != _lines[8])
                {
                    if(_currentLineCount < _linePerDialogue[_dialogueStep] - 1)
                    {
                        _lineIndex++;
                        _currentLineCount++;
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
                            _introTimeDifference = Time.realtimeSinceStartup;
                            _audioSourceIslandLoop.Play();
                            _dialogueCollider.center = new Vector3(-0.65f, 0, -46.97f);
                            _dialogueCollider.radius = 2.9f;
                            _dialogueStep++;
                            _lineIndex++;
                            _currentLineCount++;

                        }
                        else if(_dialogueStep == 1)
                        {
                            if(_nbChest > 0)
                            {
                                _dialogueStep++;
                            }
                            else
                            {
                                _lineIndex -= 2;
                            }
                        }
                        _canvasGroupHUD.alpha = 1;
                        _canvasGroupMap.alpha = 1;
                        if(!_gameRunEnded)
                        {
                            _canvasGroupPelican.alpha = 1;
                        }
                        _canvasGroupDialogueButton.interactable = false;
                        _inDialogue = false;
                        _currentLineCount = 0;
                    }

                }
            }
            else
            {
                StopAllCoroutines();
                _dialogueText.text = _lines[_lineIndex];

                if(_lineIndex == 8)
                {
                    _canvasGroupDialogueChoices.interactable = true;
                    _canvasGroupDialogueChoices.alpha = 1;
                }

            }
        }

        public void StayOnIsland()
        {
            StartCoroutine(CloseDialogue());
            //_lineIndex -= 3;
        }

        public void LeaveIsland()
        {
            _gameRunEnded = true;        
            StartCoroutine(CloseDialogue());
            StartCoroutine(SceneTransition());

        }

        IEnumerator SceneTransition()
        {
            _canvasGroupHUD.alpha = 0;
            _canvasGroupMap.alpha = 0;
            _canvasGroupCurse.alpha = 0;  
            HideScene();
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);   
         
            yield return null;
        }

        IEnumerator CloseDialogue()
        {
            _canvasGroupDialogueChoices.interactable = false;
            _canvasGroupDialogueButton.interactable = false;

            if(_lineIndex == 8)
            {
                yield return new WaitForSeconds(0.5f);
                _lineIndex -= 3;
            }
            _canvasGroupDialogueChoices.alpha = 0;
            _canvasGroupDialogueFrame.alpha = 0;
            _currentLineCount = 0;
            _dialogueText.text = string.Empty;
            _inDialogue = false;
            if(!_gameRunEnded)
            {
                _canvasGroupHUD.alpha = 1;
                _canvasGroupMap.alpha = 1;
            }
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("player"))
            {
                if(!_introIsEnded)
                {
                    _gold = 0;
                    _goldText.text = _gold.ToString("0000");
                    NewDialogue();
                }
                else
                {
                    _canvasGroupPelican.alpha = 1;
                    _nearOfPelican = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("player"))
            {
                if(_introIsEnded)
                {
                    _canvasGroupPelican.alpha = 0;
                    _nearOfPelican = false;
                }
            }
        }
        #endregion
    }
}
