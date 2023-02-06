using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// 오디오의 반복 재생을 관리하는 모듈 클래스.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class BGMLoopModule : MonoBehaviour
    {
        /// <summary>
        /// 음원을 재생할 Audio Source 객체.
        /// </summary>
        [SerializeField] private AudioSource source;

        /// <summary>
        /// 루프가 시작되는 구간.
        /// </summary>
        private float loopStart;

        /// <summary>
        /// 루프가 종료되는 구간.
        /// </summary>
        private float loopEnd;

        private void Reset()
        {
            source = GetComponentInChildren<AudioSource>();
        }

        private void Awake()
        {
            source = source ?? GetComponentInChildren<AudioSource>();

            loopStart = 0.0f;
            loopEnd = 0.999f;
        }

        private void FixedUpdate()
        {
            if (source != null && source.isPlaying == false)
                if (source.time >= loopEnd)
                    source.time = loopStart;
        }

        /// <summary>
        /// 반복 재생 데이터를 읽고, 재생 환경을 구축합니다.
        /// </summary>
        /// <param name="data"></param>
        public void SetFrom(BGMLoopData data)
        {
            source.clip = data.clip;
            loopStart = data.loopStart;
            loopEnd = data.loopEnd;
        }
    }
}