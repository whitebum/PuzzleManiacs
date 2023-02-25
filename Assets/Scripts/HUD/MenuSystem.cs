using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleManiacs.Level
{
    /// <summary>
    /// 각종 메뉴를 배열하고, 선택한 메뉴에 맞는 작업을 수행하는 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    public class MenuSystem : MonoBehaviour
    {
        #region Options
        /// <summary>
        /// Menu System의 상태를 나타내는 열거형
        /// </summary>
        public enum MenuState
        {
            None,
            Open,
            OpenComplete,
            Close,
            CloseComplete,
        }

        /// <summary>
        /// Menu System의 현재 상태.
        /// </summary>
        [SerializeField] protected MenuState currentState;

        /// <summary>
        /// 이전에 띄워졌던 Menu System.
        /// </summary>
        [SerializeField] protected MenuSystem prevMenu;
        #endregion

        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animations")]
        [Tooltip("애니메이션을 재생하는 Animator.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// Menu System이 열릴 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space]
        [Tooltip("Menu System이 열릴 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string openTrigger;

        /// <summary>
        /// Menu System이 열릴 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int openTriggerHash;

        /// <summary>
        /// Menu System이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("Menu System이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string closeTrigger;

        /// <summary>
        /// Menu System이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int closeTriggerHash;

        /// <summary>
        /// 애니메이션 종료 대기를 위한 Coroutine.
        /// </summary>
        private Coroutine animationCoroutine;
        #endregion

        #region Events
        /// <summary>
        /// Menu System이 열릴 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("Menu System이 열릴 때 실행되는 Unity Event.")]
        public UnityEvent onOpen;

        /// <summary>
        /// Menu System이 닫힐 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Menu System이 닫힐 때 실행되는 Unity Event.")]
        public UnityEvent onClose;
        #endregion

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            currentState = MenuState.None;

            animator = animator ?? GetComponent<Animator>();

            openTriggerHash = !string.IsNullOrEmpty(openTrigger) ? Animator.StringToHash(openTrigger) : 0;
            closeTriggerHash = !string.IsNullOrEmpty(closeTrigger) ? Animator.StringToHash(closeTrigger) : 0;

            onOpen = onOpen ?? new UnityEvent();
            onClose = onClose ?? new UnityEvent();
        }

        private void Update()
        {
            if (currentState == MenuState.OpenComplete)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && prevMenu != null)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// Menu System을 엽니다.
        /// </summary>
        public void Open()
        {
            gameObject.SetActive(true);
            currentState = MenuState.Open;

            onOpen.Invoke();
            if (animator != null && openTriggerHash != 0)
            {
                animator.SetTrigger(openTriggerHash);
                animator.Update(0.0f);
            }

            StopAnimationCoroutine();
            animationCoroutine = StartCoroutine(WaitForAnimationCoroutine(closeTrigger, () =>
            {
                currentState = MenuState.OpenComplete;
            }));
        }

        /// <summary>
        /// Menu System을 닫습니다.
        /// </summary>
        public void Close()
        {
            currentState = MenuState.Close;

            onClose.Invoke();
            if (animator != null && closeTriggerHash != 0)
            {
                animator.SetTrigger(closeTriggerHash);
                animator.Update(0.0f);
            }

            StopAnimationCoroutine();
            animationCoroutine = StartCoroutine(WaitForAnimationCoroutine(closeTrigger, () => 
            {
                currentState = MenuState.CloseComplete;

                gameObject.SetActive(true);

                if (prevMenu != null)
                    prevMenu.Open();
            }));
        }

        private void StopAnimationCoroutine()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        private IEnumerator WaitForAnimationCoroutine(string animation, UnityAction callback)
        {
            if (animator != null && !string.IsNullOrEmpty(animation))
            {
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f &&
                                                 animator.GetCurrentAnimatorStateInfo(0).IsName(animation));
            }

            callback.Invoke();

            yield return null;
        }
    }
}