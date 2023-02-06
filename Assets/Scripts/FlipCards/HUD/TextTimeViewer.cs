using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PuzzleExpress.HUD;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// 텍스트 형식으로 시간을 표기하는 UI.
    /// </summary>
    public sealed class TextTimeViewer : MonoBehaviour, IBaseTimeViewer
    {
        /// <summary>
        /// 시간을 표기하는 Text 객체.
        /// </summary>
        [Tooltip("시간을 띄우는 Text 객체.")]
        [SerializeField] private Text viewer;

        /// <summary>
        /// 시간을 표기할 방식.
        /// </summary>
        [Tooltip("시간을 표기할 방식.")]
        [SerializeField] private string displayFormat;

        /// <summary>
        /// UI의 애니메이터.
        /// </summary>
        [Header("Animation")]
        [Tooltip("UI의 애니메이터.")]
        [SerializeField] private Animator animator;

        /// <summary>
        /// 현재 시각에 따라 재생되는 애니메이션의 Float 파라미터.
        /// </summary>
        [Tooltip("현재 시각에 따라 재생되는 애니메이션의 Float 파라미터.")]
        [SerializeField] private string secondsBool;
        private int secondsBoolHash;

        private void Reset()
        {
            viewer = GetComponentsInChildren<Text>()[1];
            displayFormat = "mm:ss";

            animator = GetComponentInChildren<Animator>();
        }

        private void Awake()
        {
            viewer = viewer ?? GetComponentsInChildren<Text>()[1];
            animator = animator ?? GetComponentInChildren<Animator>();

            secondsBoolHash = !string.IsNullOrEmpty(secondsBool) ? Animator.StringToHash(secondsBool) : 0;
        }

        public void Display(TimeSpan time, float maxTime)
        {
            if (animator != null && secondsBoolHash != 0)
            {
                var caution = (float)time.TotalSeconds >= (maxTime - 60.0f) ? true : false;

                animator.SetBool(secondsBoolHash, caution);
                animator.Update(0.0f);
            }

            if (viewer != null)
                viewer.text = new DateTime().Add(time).ToString(string.Format(displayFormat));
        }
    }
}
