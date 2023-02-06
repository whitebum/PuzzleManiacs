using PuzzleExpress.FlipCards.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// 게임 내 띄워지는 모든 HUD 객체들의 동작을 관리하는 클래스.
    /// </summary>
    public sealed class LevelHUDManager : MonoBehaviour
    {
        /// <summary>
        /// 현재 스테이지를 관리하는 매니저 객체.
        /// </summary>
        [Tooltip("현재 스테이지를 관리하는 매니저 객체.")]
        public LevelManager level;

        /// <summary>
        /// 현재 게임 내에 띄워지고 있는 모든 HUD 객체.
        /// </summary>
        [Tooltip("현재 게임 내에 띄워지고 있는 모든 HUD 객체.")]
        public LevelHUD hud;

        private void Reset()
        {
            level = FindObjectOfType<LevelManager>();
            hud = FindObjectOfType<LevelHUD>();
        }

        private void Awake()
        {
            if (level != null) level = FindObjectOfType<LevelManager>();
            if (hud != null) hud = FindObjectOfType<LevelHUD>();
        }

        private void Start()
        {
            UpdateAllHUD();
        }

        private void Update()
        {
            if (hud != null && hud.timeViewer != null)
                hud.timeViewer.Display(level.time, level.timeOverSeconds);
        }

        private void UpdateAllHUD()
        {
            if (hud != null)
            {
                UpdateTimeViewer();
                UpdateScoreViewer();
            }
        }

        private void UpdateTimeViewer()
        {
            if (hud != null && hud.timeViewer != null)
            {
                hud.timeViewer.Display(level.time, level.timeOverSeconds);
            }
        }

        private void UpdateScoreViewer()
        {
            if (hud != null && hud.scoreViewer != null)
            {
                hud.scoreViewer.Display(level.score);
                hud.topScoreviewer.Display(level.score);
            }
        }
    }
}
