using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleExpress.Level;

namespace PuzzleExpress.FlipCards.Level
{
    /// <summary>
    /// ���� ���������� ������ �� ������ �����ϴ� Ŭ����.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// �÷��̾��� ���ھ �����ϴ� Score Counter ��ü.
        /// </summary>
        [Tooltip("�÷��̾��� ���ھ �����ϴ� Score Counter ��ü.")]
        [SerializeField] private ScoreCounter scoreCounter;

        /// <summary>
        /// �ð��� �����ϴ� Time Counter ��ü.
        /// </summary>
        [Tooltip("")]
        [SerializeField] private TimeCounter timer;

        /// <summary>
        /// ���� �ð�.
        /// </summary>
        [Tooltip("���� �ð�.")]
        [SerializeField] private float _timeOverSeconds;

        /// <summary>
        /// ���ݱ��� ����� �÷��̾��� ���ھ�.
        /// </summary>
        public int score
        {
            get { return scoreCounter != null ? scoreCounter.currentScore : 0; }
            set { if (scoreCounter != null) scoreCounter.currentScore = value; }
        }

        /// <summary>
        /// ���ݱ��� ����� �÷��̾��� ž ���ھ�.
        /// </summary>
        public int topScore
        {
            get { return scoreCounter != null ? scoreCounter.topScore : 0; }
            set { if (scoreCounter != null) scoreCounter.topScore = value; }
        }

        /// <summary>
        /// ���ݱ��� ����� �ð�.
        /// </summary>
        public TimeSpan time
        {
            get { return timer != null ? timer.currentTime : TimeSpan.Zero; }
            set { if (timer != null) timer.currentTime = value; }
        }

        /// <summary>
        /// ���ݱ��� ����� �ð�(�� ����).
        /// </summary>
        public float timeSeconds
        {
            get { return timer != null ? (float)timer.currentTime.TotalSeconds : 0.0f; }
            set { if (timer != null) timer.currentTime = TimeSpan.FromSeconds(value); }
        }

        /// <summary>
        /// ���� �ð�.
        /// </summary>
        public float timeOverSeconds
        {
            get { return _timeOverSeconds; }
        }

        /// <summary>
        /// ������ ���� ī�� ������.
        /// </summary>
        private int _leftCardsCount;

        /// <summary>
        /// ������ ���� ī�� ������.
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