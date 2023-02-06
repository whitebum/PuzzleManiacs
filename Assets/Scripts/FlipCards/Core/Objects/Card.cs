using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleExpress.FlipCards.Core.Objects
{
    /// <summary>
    /// 카드 오브젝트.
    /// </summary>
    public sealed class Card : MonoBehaviour
    {
        /// <summary>
        /// 현재 카드의 상태.
        /// </summary>
        public CardState state;

        /// <summary>
        /// 해당 카드의 태그.
        /// </summary>
        public CardType type;

        #region Animations
        /// <summary>
        /// 카드 객체의 애니메이터.
        /// </summary>
        [Header("Animations")]
        [Tooltip("카드 객체의 애니메이터.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 카드를 뒤집는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.
        /// </summary>
        [Tooltip("카드를 뒤집는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.")]
        [SerializeField] private string flipToFrontSideTrigger;
        private int flipToFrontSideTriggerHash;

        /// <summary>
        /// 카드를 뒤집는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.
        /// </summary>
        [Tooltip("카드를 뒤집는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.")]
        [SerializeField] private string flipToBackSideTrigger;
        private int flipToBackSideTriggerHash;

        /// <summary>
        /// 카드가 사라지는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.
        /// </summary>
        [Tooltip("카드가 사라지는 애니메이션을 재생하는 애니메이터의 Trigger 파라미터.")]
        [SerializeField] private string eraseTrigger;
        private int eraseTriggerHash;

        /// <summary>
        /// 현재 애니메이션이 재생되고 있는가의 유무.
        /// </summary>
        [SerializeField] private bool isPlaying;
        #endregion

        #region Events
        /// <summary>
        /// 해당 카드가 선택되었을 때 실행되는 Unity Event.
        /// </summary>
        [HideInInspector] public UnityEvent onSelected;

        /// <summary>
        /// 해당 카드가 지워졌을 때 실행되는 Unity Event.
        /// </summary>
        [HideInInspector] public UnityEvent onErased;
        #endregion

        private Coroutine coroutine;

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Awake()
        {
            animator = animator ?? GetComponentInChildren<Animator>();

            flipToFrontSideTriggerHash = !string.IsNullOrEmpty(flipToFrontSideTrigger) ? Animator.StringToHash(flipToFrontSideTrigger) : 0;
            flipToBackSideTriggerHash = !string.IsNullOrEmpty(flipToBackSideTrigger) ? Animator.StringToHash(flipToBackSideTrigger) : 0;
            eraseTriggerHash = !string.IsNullOrEmpty(eraseTrigger) ? Animator.StringToHash(eraseTrigger) : 0;

            onSelected = onSelected ?? new UnityEvent();
            onErased = onErased ?? new UnityEvent();

            state = CardState.None;
            isPlaying = false;
        }

        private void Start()
        {
            FlipToBackSide();
        }

        private void OnMouseDown()
        {
            FlipToFrontSide();
        }

        /// <summary>
        /// 해당 카드를 윗면이 하늘을 보게끔 뒤집습니다.
        /// </summary>
        public void FlipToFrontSide()
        {
            if (state != CardState.Front && isPlaying == false)
            {
                state = CardState.Front;

                if (animator != null && flipToFrontSideTriggerHash != 0)
                {
                    animator.SetTrigger(flipToFrontSideTriggerHash);
                    animator.Update(0.0f);
                }

                coroutine = StartCoroutine(WaitForAnimationCoroutine(flipToFrontSideTrigger));
            }
        }

        /// <summary>
        /// 해당 카드를 뒷면이 하늘을 보게끔 뒤집습니다.
        /// </summary>
        public void FlipToBackSide()
        {
            if (state != CardState.Back && isPlaying == false)
            {
                state = CardState.Back;

                if (animator != null && flipToBackSideTriggerHash != 0)
                {
                    animator.SetTrigger(flipToBackSideTriggerHash);
                    animator.Update(0.0f);
                }

                coroutine = StartCoroutine(WaitForAnimationCoroutine(flipToBackSideTrigger));
            }
        }

        /// <summary>
        /// 해당 카드를 지웁니다.
        /// </summary>
        public void Erase()
        {
            if (state != CardState.Erase && isPlaying == false)
            {
                state = CardState.Erase;

                if (animator != null && eraseTriggerHash != 0)
                {
                    animator.SetTrigger(eraseTriggerHash);
                    animator.Update(0.0f);
                }

                coroutine = StartCoroutine(WaitForAnimationCoroutine(eraseTrigger, () => gameObject.SetActive(false)));
            }
        }

        /// <summary>
        /// 현재 재생되는 애니메이션의 종료를 기다리고, Unity Action을 실행하는 코루틴.
        /// </summary>
        /// <param name="action">애니메이션 종료 후, 실행할 Unity Action.</param>
        /// <returns></returns>
        private IEnumerator WaitForAnimationCoroutine(string triggerName, UnityAction action = null)
        {
            if (animator != null && !string.IsNullOrEmpty(flipToBackSideTrigger))
            {
                isPlaying = true;

                yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f &&
                                                animator.GetCurrentAnimatorStateInfo(0).IsName(triggerName));

                isPlaying = false;
            }

            if (action != null) action.Invoke();

            yield return null;
        }
    }
}