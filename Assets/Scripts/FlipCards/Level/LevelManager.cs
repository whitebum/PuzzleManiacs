using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleExpress.Level;

namespace PuzzleExpress.FlipCards.Level
{
    /// <summary>
    /// 현재 스테이지의 데이터 및 루프를 관리하는 클래스.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// 플레이어의 스코어를 집계하는 Score Counter 객체.
        /// </summary>
        [Tooltip("플레이어의 스코어를 집계하는 Score Counter 객체.")]
        [SerializeField] private ScoreCounter scoreCounter;

        /// <summary>
        /// 시간을 집계하는 Time Counter 객체.
        /// </summary>
        [Tooltip("")]
        [SerializeField] private TimeCounter timer;

        /// <summary>
        /// 제한 시간.
        /// </summary>
        [Tooltip("제한 시간.")]
        [SerializeField] private float _timeOverSeconds;

        /// <summary>
        /// 지금까지 집계된 플레이어의 스코어.
        /// </summary>
        public int score
        {
            get { return scoreCounter != null ? scoreCounter.currentScore : 0; }
            set { if (scoreCounter != null) scoreCounter.currentScore = value; }
        }

        /// <summary>
        /// 지금까지 집계된 플레이어의 탑 스코어.
        /// </summary>
        public int topScore
        {
            get { return scoreCounter != null ? scoreCounter.topScore : 0; }
            set { if (scoreCounter != null) scoreCounter.topScore = value; }
        }

        /// <summary>
        /// 지금까지 집계된 시간.
        /// </summary>
        public TimeSpan time
        {
            get { return timer != null ? timer.currentTime : TimeSpan.Zero; }
            set { if (timer != null) timer.currentTime = value; }
        }

        /// <summary>
        /// 지금까지 집계된 시간(초 단위).
        /// </summary>
        public float timeSeconds
        {
            get { return timer != null ? (float)timer.currentTime.TotalSeconds : 0.0f; }
            set { if (timer != null) timer.currentTime = TimeSpan.FromSeconds(value); }
        }

        /// <summary>
        /// 제한 시간.
        /// </summary>
        public float timeOverSeconds
        {
            get { return _timeOverSeconds; }
        }

        /// <summary>
        /// 앞으로 남은 카드 묶음들.
        /// </summary>
        private int _leftCardsCount;

        /// <summary>
        /// 앞으로 남은 카드 묶음들.
        /// </summary>
        public int leftCardsCount
        {
            get { return _leftCardsCount; }
            set
            {
                _leftCardsCount = value;

                if (_leftCardsCount <= 0)
                {
                    Debug.Log("Game Over!");
                }
            }
        }

        private void Reset()
        {
            scoreCounter = FindObjectOfType<ScoreCounter>();
            timer = FindObjectOfType<TimeCounter>();
        }

        private void Awake()
        {
            if (scoreCounter != null) scoreCounter = FindObjectOfType<ScoreCounter>();
            if (timer != null) timer = FindObjectOfType<TimeCounter>();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            // Time Over
            if (timeSeconds <= timeOverSeconds)
            {
                
            }
        }
    }
}