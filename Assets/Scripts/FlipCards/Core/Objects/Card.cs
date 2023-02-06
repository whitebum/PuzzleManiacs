using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleExpress.FlipCards.Core.Objects
{
    /// <summary>
    /// ī�� ������Ʈ.
    /// </summary>
    public sealed class Card : MonoBehaviour
    {
        /// <summary>
        /// ���� ī���� ����.
        /// </summary>
        public CardState state;

        /// <summary>
        /// �ش� ī���� �±�.
        /// </summary>
        public CardType type;

        #region Animations
        /// <summary>
        /// ī�� ��ü�� �ִϸ�����.
        /// </summary>
        [Header("Animations")]
        [Tooltip("ī�� ��ü�� �ִϸ�����.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// ī�带 ������ �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.
        /// </summary>
        [Tooltip("ī�带 ������ �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.")]
        [SerializeField] private string flipToFrontSideTrigger;
        private int flipToFrontSideTriggerHash;

        /// <summary>
        /// ī�带 ������ �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.
        /// </summary>
        [Tooltip("ī�带 ������ �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.")]
        [SerializeField] private string flipToBackSideTrigger;
        private int flipToBackSideTriggerHash;

        /// <summary>
        /// ī�尡 ������� �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.
        /// </summary>
        [Tooltip("ī�尡 ������� �ִϸ��̼��� ����ϴ� �ִϸ������� Trigger �Ķ����.")]
        [SerializeField] private string eraseTrigger;
        private int eraseTriggerHash;

        /// <summary>
        /// ���� �ִϸ��̼��� ����ǰ� �ִ°��� ����.
        /// </summary>
        [SerializeField] private bool isPlaying;
        #endregion

        #region Events
        /// <summary>
        /// �ش� ī�尡 ���õǾ��� �� ����Ǵ� Unity Event.
        /// </summary>
        [HideInInspector] public UnityEvent onSelected;

        /// <summary>
        /// �ش� ī�尡 �������� �� ����Ǵ� Unity Event.
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
        /// �ش� ī�带 ������ �ϴ��� ���Բ� �������ϴ�.
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
        /// �ش� ī�带 �޸��� �ϴ��� ���Բ� �������ϴ�.
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
        /// �ش� ī�带 ����ϴ�.
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
        /// ���� ����Ǵ� �ִϸ��̼��� ���Ḧ ��ٸ���, Unity Action�� �����ϴ� �ڷ�ƾ.
        /// </summary>
        /// <param name="action">�ִϸ��̼� ���� ��, ������ Unity Action.</param>
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