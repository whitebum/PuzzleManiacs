using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// ���� �� ������� ��� HUD ��ü.
    /// </summary>
    public sealed class LevelHUD : MonoBehaviour
    {
        /// <summary>
        /// �÷��̾��� ���ھ ǥ���ϴ� UI.
        /// </summary>
        [Tooltip("�÷��̾��� ���ھ ǥ���ϴ� UI.")]
        public TextScoreViewer scoreViewer;

        /// <summary>
        /// �ְ� ���ھ ǥ���ϴ� UI.
        /// </summary>
        [Tooltip("�ְ� ���ھ ǥ���ϴ� UI.")]
        public TextScoreViewer topScoreviewer;

        /// <summary>
        /// ���� �ð��� ǥ���ϴ� UI.
        /// </summary>
        [Tooltip("���� �ð��� ǥ���ϴ� UI.")]
        public TextTimeViewer timeViewer;
    }
}