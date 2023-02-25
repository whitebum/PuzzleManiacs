using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleManiacs.HUD
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenToCloseTransition : MonoBehaviour
    {
        #region Options
        /// <summary>
        /// Transition의 상태를 나타내는 열거형.
        /// </summary>
        protected enum TransitionState
        {
            None,
            Open,
            OpenComplete,
            Close,
            CloseComplete,
        }

        /// <summary>
        /// Transition의 현재 상태.
        /// </summary>
        [Header("Options")]
        [Tooltip("Transition의 현재 상태.")]
        [SerializeField] protected TransitionState currentState;

        /// <summary>
        /// Transition 진입 완료 후, 자동으로 탈출할 것인지에 대한 여부.
        /// </summary>
        [Tooltip("Transition 진입 완료 후, 자동으로 탈출할 것인지에 대한 여부.")]
        [SerializeField] protected bool isAutometicExit;
        #endregion

        #region Animations
        /// <summary>
        /// 애니메이션을 재생하는 Animator.
        /// </summary>
        [Header("Animation")]
        [Tooltip("")]
        [SerializeField] protected Animator animator;

        /// <summary>
        /// Transition 열릴 시 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Space]
        [Tooltip("Transition 열릴 시 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string openTrigger;

        /// <summary>
        /// Transition이 열릴 시 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        private int openTriggerHash;

        /// <summary>
        /// Transition이 열렸을 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("Transition이 열렸을 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string openCompleteTrigger;

        /// <summary>
        /// Transition이 열렸을 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        private int openCompleteTriggerHash;

        /// <summary>
        /// Transition이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("Transition이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string closeTrigger;

        /// <summary>
        /// Transition이 닫힐 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int closeTriggerHash;

        /// <summary>
        /// Transition이 닫혔을 때 재생되는 애니메이션의 Trigger 파라미터.
        /// </summary>
        [Tooltip("Transition이 닫혔을 때 재생되는 애니메이션의 Trigger 파라미터.")]
        [SerializeField] private string closeCompleteTrigger;

        /// <summary>
        /// Transition이 닫혔을 때 재생되는 애니메이션의 Trigger 파라미터 해쉬.
        /// </summary>
        protected int closeCompleteTriggerHash;

        /// <summary>
        /// 애니메이션 종료 대기를 위한 Coroutine.
        /// </summary>
        protected Coroutine animationCoroutine;
        #endregion

        #region Modules
        /// <summary>
        /// Transition이 완전히 열릴 때까지의 Wait For Transition Module.
        /// </summary>
        [Header("Modules")]
        [Tooltip("Transition이 완전히 열릴 때까지의 Wait For Transition Module.")]
        [SerializeField] protected WaitForTransitionModule openToOpenComplete;

        /// <summary>
        /// Transition이 완전히 열리고 닫히기까지의 Wait For Transition Module.
        /// </summary>
        [Tooltip("Transition이 완전히 열리고 닫히기까지의 Wait For Transition Module.")]
        [SerializeField] protected WaitForTransitionModule openCompleteToClose;

        /// <summary>
        /// Transition이 완전히 닫힐 때까지의 Wait For Transition Module.
        /// </summary>
        [Tooltip("Transition이 완전히 닫힐 때까지의 Wait For Transition Module.")]
        [SerializeField] protected WaitForTransitionModule closeToCloseComplete;
        #endregion

        #region Events
        /// <summary>
        /// Transition이 열릴 때 실행되는 Unity Event.
        /// </summary>
        [Header("Events")]
        [Tooltip("Transition이 열릴 때 실행되는 Unity Event.")]
        [SerializeField] public UnityEvent onOpen;

        /// <summary>
        /// Transition이 열렸을 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Transition이 열렸을 때 실행되는 Unity Event.")]
        [SerializeField] public UnityEvent onOpenComplete;

        /// <summary>
        /// Transition이 닫힐 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Transition이 닫힐 때 실행되는 Unity Event.")]
        [SerializeField] public UnityEvent onClose;

        /// <summary>
        /// Transition이 닫혔을 때 실행되는 Unity Event.
        /// </summary>
        [Tooltip("Transition이 닫혔을 때 실행되는 Unity Event.")]
        [SerializeField] public UnityEvent onCloseComplete;
        #endregion

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Awake()
        {
            currentState = TransitionState.CloseComplete;

            animator = animator ?? GetComponentInChildren<Animator>();

            openTriggerHash = !string.IsNullOrEmpty(openTrigger) ? Animator.StringToHash(openTrigger) : 0;
            openCompleteTriggerHash = !string.IsNullOrEmpty(openCompleteTrigger) ? Animator.StringToHash(openCompleteTrigger) : 0;
            closeTriggerHash = !string.IsNullOrEmpty(closeTrigger) ? Animator.StringToHash(closeTrigger) : 0;
            closeCompleteTriggerHash = !string.IsNullOrEmpty(closeCompleteTrigger) ? Animator.StringToHash(closeCompleteTrigger) : 0;

            onOpen = onOpen ?? new UnityEvent();
            onOpenComplete = onOpenComplete ?? new UnityEvent();
            onClose = onClose ?? new UnityEvent();
            onCloseComplete = onCloseComplete ?? new UnityEvent();
        }

        /// <summary>
        /// Transition에 진입합니다.
        /// </summary>
        public virtual void Open()
        {
            gameObject.SetActive(true);
            currentState = TransitionState.Open;

            onOpen.Invoke();
            if (animator != null && openTriggerHash != 0)
            {
                animator.SetTrigger(openTriggerHash);
                animator.Update(0.0f);
            }

            StopTransitionCoroutine();
            if (openToOpenComplete != default)
                animationCoroutine = StartCoroutine(WaitForTransitionCoroutine(openToOpenComplete, OpenComplete));
        }

        /// <summary>
        /// Transition 진입을 완료합니다.
        /// </summary>
        public virtual void OpenComplete()
        {
            currentState = TransitionState.OpenComplete;

            onOpenComplete?.Invoke();
            if (animator != null && openCompleteTriggerHash != 0)
            {
                animator.SetTrigger(openCompleteTriggerHash);
                animator.Update(0.0f);
            }

            StopTransitionCoroutine();
            if (isAutometicExit == true && openCompleteToClose != default)
                animationCoroutine = StartCoroutine(WaitForTransitionCoroutine(openCompleteToClose, Close));
        }

        /// <summary>
        /// Transition을 탈출합니다.
        /// </summary>
        public virtual void Close()
        {
            currentState = TransitionState.Close;

            onClose?.Invoke();
            if (animator != null && closeTriggerHash != 0)
            {
                animator.SetTrigger(closeTriggerHash);
                animator.Update(0.0f);
            }

            StopTransitionCoroutine();
            if (closeToCloseComplete != default)
                animationCoroutine = StartCoroutine(WaitForTransitionCoroutine(closeToCloseComplete, CloseComplete));
        }

        /// <summary>
        /// Transition 탈출을 완료합니다.
        /// </summary>
        public virtual void CloseComplete()
        {
            currentState = TransitionState.CloseComplete;

            onCloseComplete?.Invoke();
            if (animator != null && closeCompleteTriggerHash != 0)
            {
                animator.SetTrigger(closeCompleteTriggerHash);
                animator.Update(0.0f);
            }

            StopTransitionCoroutine();
        }

        /// <summary>
        /// 현재 재생 중인 Transition 애니메이션을 중지합니다.
        /// </summary>
        public void StopTransitionCoroutine()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        protected virtual IEnumerator WaitForTransitionCoroutine(WaitForTransitionModule module, UnityAction callback)
        {
            if (module.time > 0.0f)
            {
                yield return new WaitForSeconds(module.time);
            }
            else if (!string.IsNullOrEmpty(module.animation) && animator != null)
            {
                yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.999f &&
                                                 animator.GetCurrentAnimatorStateInfo(0).IsName(module.animation));
            }
            else if (module.transition != null)
            {
                yield return new WaitWhile(() => module.transition.currentState == TransitionState.CloseComplete);
            }
            //else if (module.audio != null)
            //{
            //    AudioManager.Instance.StopBGM();
            //    AudioManager.Instance.PlayJingle(trigger.Sound);
            //    yield return new WaitWhile(() => SoundManager.Instance.JingleSource.isPlaying);
            //}
            else
            {
                yield break;
            }

            callback.Invoke();
        }
    }
}
