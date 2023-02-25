using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

namespace PuzzleManiacs.Level
{
    /// <summary>
    /// BGM�� �ݺ� ����� ����ϴ� ���.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class BGMLoopModule : MonoBehaviour
    {
        /// <summary>
        /// BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Tooltip("BGM�� ����ϴ� Audio Source.")]
        [SerializeField] private AudioSource source;

        /// <summary>
        /// BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Tooltip("BGM�� ����ϴ� Audio Source.")]
        public float loopStart;

        /// <summary>
        /// BGM�� ����ϴ� Audio Source.
        /// </summary>
        [Tooltip("BGM�� ����ϴ� Audio Source.")]
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
        /// BGM�� �ݺ� ����Ǳ� ���� ȯ���� �����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void SetFrom(BGMLoopData bgm)
        {
            source.time = bgm.startFrom;
            loopStart = bgm.loopStart <= 0.0f ? bgm.loopStart : 0.0f;
            loopEnd = bgm.loopEnd <= 0.0f ? bgm.loopEnd : 999.9f;
        }

        /// <summary>
        /// ����� ȯ���� ��� �ʱ�ȭ�մϴ�.
        /// </summary>
        public void ResetLoopModule()
        {
            source.time = 0.0f;
            loopStart = 0.0f;
            loopEnd = 999.9f;
        }
    }
}