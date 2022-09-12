using UnityEngine;

namespace caca
{
    public class Game_Manager : MonoBehaviour
    {
        #region Inspector

        [Header("Others")]
        public Transform _timerLeft;
        public Transform _timerRight;

        [Header("Game Manager")]
        public int _limitTime;

        #endregion


        #region Hidden

        private int _timeTemp = 9999;

        private float _startTime;

        #endregion


        #region Updates

        private void Start()
        {
            _startTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            TimeManagement();
        }

        #endregion


        #region main

        public void TimeManagement()
        {
            int seconds = Mathf.RoundToInt(Time.realtimeSinceStartup) - Mathf.RoundToInt(_startTime);
            int timeLeft = _limitTime - seconds;

            if (timeLeft > -1 && timeLeft < _timeTemp)
            {
                //_timerLeft.position = new Vector3((_timerLeft.position.x + ((1 / _factor) * 0.5f)), _timerLeft.position.y, _timerLeft.position.z);
                //_timerLeft.localPosition = new Vector3((_timerLeft.position.x + (1 / _factor)), _timerLeft.position.y, _timerLeft.position.z);
                //_timerRight.position = new Vector3((_timerLeft.position.x - (1 / _factor)), _timerLeft.position.y, _timerLeft.position.z);

                _timeTemp = timeLeft;
            }
        }

        #endregion
    }
}
