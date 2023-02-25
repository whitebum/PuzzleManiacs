using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleManiacs.Core;
using PuzzleManiacs.Level;

namespace PuzzleManiacs.HUD
{
    /// <summary>
    /// 타이틀 로고, 개발자 로고와 같은 UI를 포함하는 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TitleScreen : OpenToCloseTransition
    {
        #region Options
        /// <summary>
        /// Title Screen의 메인 BGM.
        /// </summary>
        [Header("Options")]
        [Tooltip("Title Screen의 메인 BGM.")]
        [SerializeField] private BGMLoopData titleTheme;

        /// <summary>
        /// 게임 선택, 도움말 보기, 게임 종료와 같은 메뉴들을 포함하는 초기 Menu System.
        /// </summary>
        [Tooltip("게임 선택, 도움말 보기, 게임 종료와 같은 메뉴들을 포함하는 초기 Menu System.")]
        [SerializeField] private MenuSystem mainMenu;
        #endregion

        private void Start()
        {
            AudioManager.Instance.PlayBGM(titleTheme);

            Open();
        }

        private void Update()
        {
            if (Input.GetKeyDown(InputUtility.SelectKey))
            {
                switch (currentState) 
                {
                    case TransitionState.Open:
                        {
                            OpenComplete();
                        }
                        break;
                    case TransitionState.OpenComplete:
                        {
                            AudioManager.Instance.StopBGM();

                            Close();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void CloseComplete()
        {
            currentState = TransitionState.CloseComplete;
            onCloseComplete?.Invoke();
            StopTransitionCoroutine();
            mainMenu.Open();
            gameObject.SetActive(false);
        }
    }
}