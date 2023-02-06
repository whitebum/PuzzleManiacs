using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// 반복 재생할 음원의 데이터 테이블.
    /// </summary>
    public class BGMLoopData : ScriptableObject
    {
        /// <summary>
        /// 반복 재생할 음원.
        /// </summary>
        [Tooltip("반복 재생할 음원.")]
        public AudioClip clip;

        /// <summary>
        /// 루프가 시작되는 구간.
        /// </summary>
        [Tooltip("루프가 시작되는 구간.")]
        public float loopStart;

        /// <summary>
        /// 루프가 끝나는 구간.
        /// </summary>
        [Tooltip("루프가 끝나는 구간.")]
        public float loopEnd;
    }
}
