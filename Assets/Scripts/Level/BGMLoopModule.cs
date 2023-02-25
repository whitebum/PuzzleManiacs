using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace PuzzleManiacs.Level
{
    /// <summary>
    /// BGM의 반복 재생을 담당하는 모듈.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class BGMLoopModule : MonoBehaviour
    {
        /// <summary>
        /// BGM을 재생하는 Audio Source.
        /// </summary>
        [Tooltip("BGM을 재생하는 Audio Source.")]
        [SerializeField] private AudioSource source;

        /// <summary>
        /// BGM을 재생하는 Audio Source.
        /// </summary>
        [Tooltip("BGM을 재생하는 Audio Source.")]
        public float loopStart;

        /// <summary>
        /// BGM을 재생하는 Audio Source.
        /// </summary>
        [Tooltip("BGM을 재생하는 Audio Source.")]
        public float loopEnd;

        private void Reset()
        {
            source = GetComponentInChildren<AudioSource>();
            ResetLoopModule();
        }

        private void Awake()
        {
            source = source ?? GetComponentInChildren<AudioSource>();
        }

        private void FixedUpdate()
        {
            if (source != null && 
                source.isPlaying == true && 
                source.time >= loopEnd)
            {
                source.time = loopStart;
            }
        }

        /// <summary>
        /// BGM이 반복 재생되기 위한 환경을 구축합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void SetFrom(BGMLoopData bgm)
        {
            source.time = bgm.startFrom;
            loopStart = bgm.loopStart <= 0.0f ? bgm.loopStart : 0.0f;
            loopEnd = bgm.loopEnd <= 0.0f ? bgm.loopEnd : 999.9f;
        }

        /// <summary>
        /// 구축된 환경을 모두 초기화합니다.
        /// </summary>
        public void ResetLoopModule()
        {
            source.time = 0.0f;
            loopStart = 0.0f;
            loopEnd = 999.9f;
        }
    }
}