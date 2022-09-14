using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        [Header("Game Manager")]
        public int _limitTime;
        public int _disappearingTime;

        #endregion


        #region Hidden

        private float _startTime;
        private float _tempLimitTime;
        private float _tempDisappearingTime;

        #endregion


        #region Updates

        private void Start()
        {
            _startTime = Time.realtimeSinceStartup;
            _tempLimitTime = (float)_limitTime;
            _tempDisappearingTime = (float)_disappearingTime;
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
            }
        }

        #endregion


        #region Utils

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        #endregion
    }
}
