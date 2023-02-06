using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// ������� �ݺ� ����� �����ϴ� ��� Ŭ����.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class BGMLoopModule : MonoBehaviour
    {
        /// <summary>
        /// ������ ����� Audio Source ��ü.
        /// </summary>
        [SerializeField] private AudioSource source;

        /// <summary>
        /// ������ ���۵Ǵ� ����.
        /// </summary>
        private float loopStart;

        /// <summary>
        /// ������ ����Ǵ� ����.
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
        /// �ݺ� ��� �����͸� �а�, ��� ȯ���� �����մϴ�.
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