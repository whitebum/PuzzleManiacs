using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// 게임 내 띄워지는 모든 HUD 객체.
    /// </summary>
    public sealed class LevelHUD : MonoBehaviour
    {
        /// <summary>
        /// 플레이어의 스코어를 표기하는 UI.
        /// </summary>
        [Tooltip("플레이어의 스코어를 표기하는 UI.")]
        public TextScoreViewer scoreViewer;

        /// <summary>
        /// 최고 스코어를 표기하는 UI.
        /// </summary>
        [Tooltip("최고 스코어를 표기하는 UI.")]
        public TextScoreViewer topScoreviewer;

        /// <summary>
        /// 현재 시간을 표기하는 UI.
        /// </summary>
        [Tooltip("현재 시간을 표기하는 UI.")]
        public TextTimeViewer timeViewer;
    }
}