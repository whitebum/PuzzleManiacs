using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleManiacs.Core;
using System.Linq;
using UnityEngine.Events;

namespace PuzzleManiacs.Level
{
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM Properties
        /// <summary>
        /// BGM ������ ������ �� ���Ǵ� Ű.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM�� �⺻ ����.
        /// </summary>
        private const float defalutBGMVolume = 0.3f;

        /// <summary>
        /// ���� BGM ����� Audio Source.
        /// </summary>
        [Header("BGM")]
        [Tooltip("���� BGM ����� Audio Source.")]
        public AudioSource bgmSource;

        /// <summary>
        /// ���� BGM�� �ݺ� �����Ű�� BGM Loop Module.
        /// </summary>
        [Tooltip("¡�� ����� Audio Source.")]
        [HideInInspector] public BGMLoopModule bgmLoopModule;

        /// <summary>
        /// Ư�� ��Ȳ BGM ����� Audio Source.
        /// </summary>
        [Tooltip("Ư�� ��Ȳ BGM ����� Audio Source.")]
        public AudioSource specialSource;

        /// <summary>
        /// Ư�� ��Ȳ BGM�� �ݺ� �����Ű�� BGM Loop Module.
        /// </summary>
        [Tooltip("Ư�� ��Ȳ BGM�� �ݺ� �����Ű�� BGM Loop Module.")]
        [HideInInspector] public BGMLoopModule specialLoopModule;

        /// <summary>
        /// ¡�� ����� Audio Source.
        /// </summary>
        [Tooltip("¡�� ����� Audio Source.")]
        public AudioSource jingleSource;

        /// <summary>
        /// ���� ������ SFX ����.
        /// </summary>
        private float bgmVolume
        {
            get => PlayerPrefs.HasKey(bgmVolumeKey) ? PlayerPrefs.GetFloat(bgmVolumeKey) : defalutBGMVolume;
            set
            {
                if (value > 0.0f)
                {
                    bgmSource.volume = value;
                    specialSource.volume = value;
                    jingleSource.volume = value;

                    PlayerPrefs.SetFloat(bgmVolumeKey, value);
                }
            }
        }
        #endregion
        #region SFX Properties
        /// <summary>
        /// SFX ������ ������ �� ���Ǵ� Ű.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX�� �⺻ ����.
        /// </summary>
        private const float defalutSFXVolume = 0.3f;

        /// <summary>
        /// ���ÿ� ��� ������ �⺻ SFX ����.
        /// </summary>
        private const int defaultMaxConcurrentSFXCount = 16;

        /// <summary>
        /// ���ÿ� ��� ������ SFX ����.
        /// </summary>
        [Header("SFX")]
        [Tooltip("���ÿ� ��� ������ SFX ����.")]
        [SerializeField] private int maxConcurrentSFXCount;

        /// <summary>
        /// SFX ����� Audio Source��.
        /// </summary>
        [Tooltip("SFX�� ����ϴ� Audio Source��.")]
        [SerializeField] private List<AudioSource> sfxSources;

        /// <summary>
        /// ���� ������ �ʴ� SFX ����� Audio Source�� ��ȯ�մϴ�.
        /// </summary>
        private AudioSource sfxSource
        {
            get
            {
                var source = sfxSources.FirstOrDefault((s) => s.isPlaying == false);

                if (source == null)
                {
                    var prevCount = maxConcurrentSFXCount + 1;
                    sfxSources.Capacity = (maxConcurrentSFXCount += 5);

                    for (int count = prevCount; count <= maxConcurrentSFXCount; ++count)
                        sfxSources.Add(CreateNewSFXSource(count));

                    return sfxSources[prevCount];
                }

                return source;
            }
        }

        /// <summary>
        /// ���� ������ SFX ����.
        /// </summary>
        private float sfxVolume
        {
            get => PlayerPrefs.HasKey(sfxVolumeKey) ? PlayerPrefs.GetFloat(sfxVolumeKey) : defalutSFXVolume;
            set
            {
                if (value > 0.0f)
                {
                    foreach (var source in sfxSources)
                        source.volume = value;

                    PlayerPrefs.SetFloat(sfxVolumeKey, value);
                }
            }
        }
        #endregion

        private Coroutine audioCoroutine;

        private enum BGMState
        {
            None,
            BGM,
            Special,
            Jingle,
        }

        private Stack<BGMState> bgmStates;

        protected override void Awake()
        {
            base.Awake();

            bgmVolume = PlayerPrefs.HasKey(bgmVolumeKey) ? PlayerPrefs.GetFloat(bgmVolumeKey) : defalutBGMVolume;

            bgmLoopModule = bgmSource.GetComponent<BGMLoopModule>() ?? bgmSource.gameObject.AddComponent<BGMLoopModule>();
            specialLoopModule = specialSource.GetComponent<BGMLoopModule>() ?? specialSource.gameObject.AddComponent<BGMLoopModule>();

            CreateSFXSources();
        }

        private void Update()
        {
            
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            PlayerPrefs.SetFloat(bgmVolumeKey, bgmVolume);
            PlayerPrefs.SetFloat(sfxVolumeKey, sfxVolume);
        }

        /// <summary>
        /// ���ο� SFX ����� Audio Source�� �����մϴ�.
        /// </summary>
        /// <param name="index">���ο� SFX ����� Audio Source�� �ε��� ��ȣ.</param>
        /// <returns></returns>
        private AudioSource CreateNewSFXSource(int index)
        {
            var newSource = new GameObject($"SFX Source ({index})").AddComponent<AudioSource>();
            newSource.transform.SetParent(transform);
            newSource.transform.SetPositionAndRotation(transform.position, transform.rotation);
            newSource.playOnAwake = false;
            newSource.spatialBlend = 0.0f;
            newSource.volume = sfxVolume;

            return newSource;
        }

        /// <summary>
        /// SFX ����� Audio Source���� �����մϴ�.
        /// </summary>
        private void CreateSFXSources()
        {
            if (maxConcurrentSFXCount <= 0) maxConcurrentSFXCount = defaultMaxConcurrentSFXCount;
            
            sfxVolume = PlayerPrefs.HasKey(sfxVolumeKey) ? PlayerPrefs.GetFloat(sfxVolumeKey) : defalutSFXVolume;
            sfxSources = new List<AudioSource>();
            sfxSources.Capacity = maxConcurrentSFXCount;

            for (int count = 1; count <= maxConcurrentSFXCount; count++)
                sfxSources.Add(CreateNewSFXSource(count));
        }

        /// <summary>
        /// ���� BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void PlayBGM(BGMLoopData bgm)
        {
            if (bgmLoopModule != null)
                bgmLoopModule.SetFrom(bgm);

            PlayBGM(bgm.clip);
        }

        /// <summary>
        /// ���� BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void PlayBGM(AudioClip bgm)
        {
            specialSource.Stop();
            jingleSource.Stop();

            bgmSource.clip = bgm;
            bgmSource.Play();
        }

        /// <summary>
        /// ���� BGM�� �Ͻ� �����մϴ�.
        /// </summary>
        public void PauseBGM()
        {
            if (bgmSource.isPlaying == true) bgmSource.Pause();
        }

        /// <summary>
        /// ���� BGM�� �����մϴ�.
        /// </summary>
        public void StopBGM()
        {
            if (bgmSource.isPlaying == true) bgmSource.Stop();
        }

        /// <summary>
        /// Audio Source�� ������ õõ�� ����, �ε巯�� ������ �����ϴ� �ڷ�ƾ.
        /// </summary>
        /// <param name="source">������ų Audio Source.</param>
        /// <param name="callback">������Ų �� ������ �ݹ�.</param>
        /// <returns></returns>
        public IEnumerator FadeOutAudioSource(AudioSource source, UnityAction callback)
        {
            if (source.isPlaying == true)
            {
                while (true)
                {
                    source.volume -= 0.3f * Time.deltaTime;

                    if (source.volume <= 0.0f)
                    {
                        source.Stop();
                        callback.Invoke();

                        yield break;
                    }

                    yield return null;
                }
            }

            yield return null;
        }

        /// <summary>
        /// Audio Source�� ������ õõ�� ����, �ε巯�� ����� �����ϴ� �ڷ�ƾ.
        /// </summary>
        /// <param name="source">�����ų Audio Source.</param>
        /// <returns></returns>
        public IEnumerator FadeInAudioSource(AudioSource source)
        {
            if (source.isPlaying == false)
            {
                source.Play();

                while (true)
                {
                    source.volume += 0.3f * Time.deltaTime;

                    if (source.volume >= bgmVolume)
                    {
                        source.volume = bgmVolume;

                        yield break;
                    }

                    yield return null;
                }
            }

            yield return null;
        }

        /// <summary>
        /// Ư�� ��Ȳ BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void PlaySpecialBGM(BGMLoopData bgm)
        {
            bgmSource.Pause();
            jingleSource.Stop();

            if (specialLoopModule != null)
                specialLoopModule.SetFrom(bgm);

            specialSource.clip = bgm.clip;
            specialSource.Play();
        }

        /// <summary>
        /// ¡�� BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
        public void PlayJingleBGM(BGMLoopData bgm)
        {
            bgmSource.Pause();
            specialSource.Pause();
        }

        public IEnumerator WaitJingleCoroutine(BGMLoopData bgm)
        {
            
            yield return null;
        }

        /// <summary>
        /// ������ ��ġ���� SFX�� ����մϴ�.
        /// </summary>
        /// <param name="sfx"></param>
        /// <param name="position"></param>
        public void PlaySFXAtPoint(AudioClip sfx, Vector3 position = default)
        {
            var source = sfxSource;
            source.clip = sfx;
            source.transform.position = position;
            source.Play();
        }

    }
}