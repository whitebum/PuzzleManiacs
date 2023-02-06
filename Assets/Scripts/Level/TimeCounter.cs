using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleExpress.Level
{
    /// <summary>
    /// 시간을 집계하는 클래스.
    /// </summary>
    public sealed class TimeCounter : MonoBehaviour
    {
        /// <summary>
        /// 시간을 집계하는 Time Span 객체.
        /// </summary>
        public TimeSpan currentTime;

        public void Update()
        {
            currentTime = currentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));
        }
    }
}
