using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleExpress.Level
{
    /// <summary>
    /// 플레이어의 스코어를 집계하고, 관리하는 클래스.
    /// </summary>
    public sealed class ScoreCounter : MonoBehaviour
    {
        /// <summary>
        /// 현재 플레이어의 스코어.
        /// </summary>
        [Tooltip("현재 플레이어의 스코어.")]
        [SerializeField] private int _currentScore;

        /// <summary>
        /// 현재 플레이어의 스코어.
        /// </summary>
        public int currentScore
        {
            get { return _currentScore; }
            set
            {
                _currentScore = value;
            }
        }

        /// <summary>
        /// 최고 스코어.
        /// </summary>
        [Tooltip("최고 스코어.")]
        [SerializeField] private int _topScore;

        /// <summary>
        /// 최고 스코어.
        /// </summary>
        public int topScore
        {
            get { return _topScore; }
            set
            {
                _topScore = value;
            }
        }

        /// <summary>
        /// 값이 바뀌었을 때 실행되는 Unity Event.
        /// </summary>
        [HideInInspector] public UnityEvent onValueChanged;

        private void Awake()
        {
            onValueChanged = new UnityEvent();
        }
    }
}
