using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.Core
{
    /// <summary>
    /// ����� ����� �����ϴ� �Ŵ��� Ŭ����.
    /// </summary>
    public sealed class AudioManager : Singleton<AudioManager>
    {
        #region BGM
        /// <summary>
        /// BGM�� ����ϴ� ����Ŀ���� �⺻ ����.
        /// </summary>
        private const float defaultBGMVolume = 0.5f;

        /// <summary>
        /// BGM�� ����ϴ� ����Ŀ���� ������ �ҷ����� Ű��.
        /// </summary>
        private const string bgmVolumeKey = "BGM Volume";

        /// <summary>
        /// BGM�� ����ϴ� ����Ŀ���� ����.
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
        /// ���� BGM�� ����ϴ� ����Ŀ.
        /// </summary>
        [Header("BGM")]
        [Tooltip("���� BGM ����Ŀ.")]
        [SerializeField] private AudioSource bgmSpeaker;

        /// <summary>
        /// ���� BGM�� �ݺ� ����� ����ϴ� ���.
        /// </summary>
        private BGMLoopModule bgmLoopModule;

        /// <summary>
        /// Ư���� ��Ȳ�� ����Ǵ� BGM�� ����ϴ� ����Ŀ.
        /// </summary>
        [Tooltip("���� BGM ����Ŀ.")]
        [SerializeField] private AudioSource specialSpeaker;

        /// <summary>
        /// Ư���� ��Ȳ�� ����Ǵ� BGM�� �ݺ� ����� ����ϴ� ���.
        /// </summary>
        private BGMLoopModule specialLoopModule;

        /// <summary>
        /// ¡���� ����ϴ� ����Ŀ.
        /// </summary>
        [Tooltip("¡�� BGM ����Ŀ.")]
        [SerializeField] private AudioSource jingleSpeaker;
        #endregion

        #region SFX
        /// <summary>
        /// SFX�� ����ϴ� ����Ŀ�� �⺻ ����.
        /// </summary>
        private const float defaultSFXVolume = 0.5f;

        /// <summary>
        /// SFX�� ����ϴ� ����Ŀ���� ������ �ҷ����� Ű��.
        /// </summary>
        private const string sfxVolumeKey = "SFX Volume";

        /// <summary>
        /// SFX�� ����ϴ� ����Ŀ�� ����.
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
        /// ���ÿ� ��� ������ SFX ���� ������ �⺻ �ѵ�.
        /// </summary>
        private const int defaultConcurrentPlayCount = 8;

        /// <summary>
        /// ���ÿ� ��� ������ SFX ���� ������ �ѵ�.
        /// </summary>
        [Header("SFX")]
        [SerializeField] private int maxConcurrentPlayCount;

        /// <summary>
        /// SFX�� ����ϴ� ����Ŀ��.
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
        /// BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
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
        /// Ư���� ��Ȳ�� ����Ǵ� BGM�� ����մϴ�.
        /// </summary>
        /// <param name="bgm">����� BGM.</param>
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
        /// ¡���� ����մϴ�.
        /// </summary>
        /// <param name="jingle">����� ¡��.</param>
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