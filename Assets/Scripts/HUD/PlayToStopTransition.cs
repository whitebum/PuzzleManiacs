using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleManiacs.HUD
{
    public class PlayToStopTransition : MonoBehaviour
    {
        #region Options 
        /// <summary>
        /// Transition의 상태를 나타내는 열거형.
        /// </summary>
        public enum TransitionState
        {
            None,
            Play,
            Pause,
            Stop,
        }

        /// <summary>
        /// Transition의 현재 상태.
        /// </summary>
        [Header("Options")]
        [Tooltip("Transition의 현재 상태.")]
        [SerializeField] protected TransitionState currentState;
        #endregion

        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("애니메이션을 재생하는 Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// Transition이 재생되고 있을 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space(2.5f)]
        [Tooltip("Transition이 재생될 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string playingBool;

        /// <summary>
        /// Transition이 재생되고 있을 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int playingBoolHash;

        /// <summary>
        /// Transition이 재생될 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space(2.5f)]
        [Tooltip("Transition이 재생될 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string playTrigger;

        /// <summary>
        /// Transition이 재생될 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int playTriggerHash;

        /// <summary>
        /// Transition이 일시 정지될 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space(2.5f)]
        [Tooltip("Transition이 일시 정지될 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string pauseTrigger;

        /// <summary>
        /// Transition이 일시 정지될 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int pauseTriggerHash;

        /// <summary>
        /// Transition이 정지될 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space(2.5f)]
        [Tooltip("Transition이 정지될 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string stopTrigger;

        /// <summary>
        /// Transition이 정지될 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int stopTriggerHash;
        #endregion

        #region Events
        /// <summary>
        /// Transition이 재생될 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("Transition이 재생될 때 실행되는 Unity Event.")]
        public UnityEvent onPlay;

        /// <summary>
        /// Transition이 일시 정지될 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Transition이 일시 정지될 때 실행되는 Unity Event.")]
        public UnityEvent onPause;

        /// <summary>
        /// Transition이 정지될 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Transition이 정지될 때 실행되는 Unity Event")]
        public UnityEvent onStop;
        #endregion

        protected virtual void Reset()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void Awake()
        {
            currentState = TransitionState.Stop;

            animator = animator ?? GetComponent<Animator>();

            playingBoolHash = !string.IsNullOrEmpty(playingBool) ? Animator.StringToHash(playingBool) : 0;
            playTriggerHash = !string.IsNullOrEmpty(playTrigger) ? Animator.StringToHash(playTrigger) : 0;
            pauseTriggerHash = !string.IsNullOrEmpty(pauseTrigger) ? Animator.StringToHash(pauseTrigger) : 0;
            stopTriggerHash = !string.IsNullOrEmpty(stopTrigger) ? Animator.StringToHash(stopTrigger) : 0;

            onPlay = onPlay ?? new UnityEvent();
            onPause = onPause ?? new UnityEvent();
            onStop = onStop ?? new UnityEvent();
        }

        protected virtual void Update()
        {
            if (currentState == TransitionState.Play)
            {
                OnPlaying();
            }
        }

        /// <summary>
        /// Transition이 재생 중일 때 실행됩니다.
        /// </summary>
        protected virtual void OnPlaying()
        {
            // None
        }

        /// <summary>
        /// Transition을 재생합니다.
        /// </summary>
        public virtual void Play()
        {
            gameObject.SetActive(true);
            currentState = TransitionState.Play;

            onPlay?.Invoke();
            if (animator != null && playTriggerHash != 0)
            {
                animator.SetTrigger(playTriggerHash);
                animator.Update(0.0f);
            }
        }

        /// <summary>
        /// Transition을 일시 정지합니다.
        /// </summary>
        public virtual void Pause()
        {
            currentState = TransitionState.Pause;

            onPause?.Invoke();
            if (animator != null && pauseTriggerHash != 0)
            {
                animator.SetTrigger(pauseTriggerHash);
                animator.Update(0.0f);
            }
        }

        /// <summary>
        /// Transition을 정지합니다.
        /// </summary>
        public virtual void Stop()
        {
            currentState = TransitionState.Stop;

            onPause?.Invoke();
            if (animator != null && pauseTriggerHash != 0)
            {
                animator.SetTrigger(pauseTriggerHash);
                animator.Update(0.0f);
            }
        }
    }
}
