using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// 오디오 재생을 관리하는 매니저 클래스.
    /// </summary>
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM
        /// <summary>
        /// BGM을 재생하는 스피커들의 기본 볼륨.
        /// </summary>
        private const float defaultBGMVolume = 0.5f;

        /// <summary>
        /// BGM을 재생하는 스피커들의 볼륨을 불러오는 키값.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM을 재생하는 스피커들의 볼륨.
        /// </summary>
        public float bgmVolume
        {
            get
            {
                if (PlayerPrefs.HasKey(bgmVolumeKey))
                    PlayerPrefs.SetFloat(bgmVolumeKey, defaultBGMVolume);

                return PlayerPrefs.GetFloat(bgmVolumeKey);
            }
            set { PlayerPrefs.SetFloat(bgmVolumeKey, value); }
        }
        /// <summary>
        /// 메인 BGM을 재생하는 스피커.
        /// </summary>
        [Header("BGM")]
        [Tooltip("메인 BGM 스피커.")]
        [SerializeField] private AudioSource bgmSpeaker;

        /// <summary>
        /// 메인 BGM의 반복 재생을 담당하는 모듈.
        /// </summary>
        private BGMLoopModule bgmLoopModule;

        /// <summary>
        /// 특별한 상황에 재생되는 BGM을 재생하는 스피커.
        /// </summary>
        [Tooltip("서브 BGM 스피커.")]
        [SerializeField] private AudioSource specialSpeaker;

        /// <summary>
        /// 특별한 상황에 재생되는 BGM의 반복 재생을 담당하는 모듈.
        /// </summary>
        private BGMLoopModule specialLoopModule;

        /// <summary>
        /// 징글을 재생하는 스피커.
        /// </summary>
        [Tooltip("징글 BGM 스피커.")]
        [SerializeField] private AudioSource jingleSpeaker;
        #endregion

        #region SFX
        /// <summary>
        /// SFX를 재생하는 스피커의 기본 볼륨.
        /// </summary>
        private const float defaultSFXVolume = 0.5f;

        /// <summary>
        /// SFX를 재생하는 스피커들의 볼륨을 불러오는 키값.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX를 재생하는 스피커의 볼륨.
        /// </summary>
        public float sfxVolume
        {
            get
            {
                if (PlayerPrefs.HasKey(sfxVolumeKey))
                    PlayerPrefs.SetFloat(sfxVolumeKey, defaultSFXVolume);

                return PlayerPrefs.GetFloat(sfxVolumeKey);
            }
            set { PlayerPrefs.SetFloat(sfxVolumeKey, value); }
        }

        /// <summary>
        /// 동시에 재생 가능한 SFX 음원 개수의 기본 한도.
        /// </summary>
        private const int defaultConcurrentPlayCount = 8;

        /// <summary>
        /// 동시에 재생 가능한 SFX 음원 개수의 한도.
        /// </summary>
        [Header("SFX")]
        [SerializeField] private int maxConcurrentPlayCount;

        /// <summary>
        /// SFX를 재생하는 스피커들.
        /// </summary>
        [SerializeField] private List<AudioSource> sfxSpeakers;
        #endregion

        private void Reset()
        {
            bgmSpeaker = transform.Find("Main Speaker").GetComponentInChildren<AudioSource>();
            bgmLoopModule = bgmSpeaker.GetComponentInChildren<BGMLoopModule>();

            specialSpeaker = transform.Find("Second Speaker").GetComponentInChildren<AudioSource>();
            specialLoopModule = specialSpeaker.GetComponentInChildren<BGMLoopModule>();

            jingleSpeaker = transform.Find("Jingle Speaker").GetComponentInChildren<AudioSource>();

            maxConcurrentPlayCount = defaultConcurrentPlayCount;
        }

        protected override void Awake()
        {
            base.Awake();

            if (bgmSpeaker == null) bgmSpeaker = transform.Find("Main Speaker").GetComponentInChildren<AudioSource>();
            if (bgmLoopModule == null) bgmLoopModule = bgmSpeaker.GetComponentInChildren<BGMLoopModule>();

            if (specialSpeaker == null) specialSpeaker = transform.Find("Second Speaker").GetComponentInChildren<AudioSource>();
            if (specialLoopModule == null) specialLoopModule = specialSpeaker.GetComponentInChildren<BGMLoopModule>();

            if (jingleSpeaker == null) jingleSpeaker = transform.Find("Jingle Speaker").GetComponentInChildren<AudioSource>();

            sfxSpeakers = new List<AudioSource>();
            sfxSpeakers.Capacity = maxConcurrentPlayCount = (maxConcurrentPlayCount > 0 ? maxConcurrentPlayCount : defaultConcurrentPlayCount);

            var parent = new GameObject("SFX Speakers").transform;
            parent.SetParent(transform);
            parent.SetPositionAndRotation(parent.position, parent.rotation);

            for (int count = 1; count <= maxConcurrentPlayCount; ++count)
            {
                var newSpeaker = new GameObject($"SFX Speaker ({count})").AddComponent<AudioSource>();
                newSpeaker.transform.SetParent(parent);
                newSpeaker.transform.SetPositionAndRotation(parent.position, parent.rotation);
                newSpeaker.playOnAwake = false;
                newSpeaker.volume = sfxVolume;

                sfxSpeakers.Add(newSpeaker);
            }
        }

        /// <summary>
        /// BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생할 BGM.</param>
        public void PlayBGM(BGMLoopData bgm)
        {
            if (bgmSpeaker != null)
            {
                bgmSpeaker.clip = bgm.clip;
                bgmLoopModule.SetFrom(bgm);

                bgmSpeaker.Play();
            }
        }

        /// <summary>
        /// 특별한 상황에 재생되는 BGM을 재생합니다.
        /// </summary>
        /// <param name="bgm">재생한 BGM.</param>
        public void PlaySpecialBGM(BGMLoopData bgm)
        {
            if (specialSpeaker != null)
            {
                specialSpeaker.clip = bgm.clip;
                specialLoopModule.SetFrom(bgm);

                specialSpeaker.Play();
            }
        }

        /// <summary>
        /// 징글을 재생합니다.
        /// </summary>
        /// <param name="jingle">재생할 징글.</param>
        public void PlayJingleBGM(AudioClip jingle)
        {
            if (jingleSpeaker != null)
            {
                jingleSpeaker.clip = jingle;
                jingleSpeaker.Play();
            }
        }
    }
}