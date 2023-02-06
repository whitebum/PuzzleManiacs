using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PuzzleExpress.HUD;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// 게이지 형식으로 시간을 표기하는 UI.
    /// </summary>
    public class GageTimeViewer : MonoBehaviour, IBaseTimeViewer
    {
        /// <summary>
        /// 시간을 표기할 게이지 이미지.
        /// </summary>
        [Tooltip("시간을 표기할 게이지 이미지.")]
        [SerializeField] private Image viewer;

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

        public void Display(TimeSpan time, float maxTime)
        {
            if (animator != null && secondsBoolHash != 0)
            {
                var caution = (float)time.TotalSeconds >= (maxTime - 60.0f) ? true : false;

                animator.SetBool(secondsBoolHash, caution);
                animator.Update(0.0f);
            }

            if (viewer != null)
                viewer.fillAmount = (float)time.TotalSeconds / maxTime;
        }
    }
}
