using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleManiacs.Core;
using PuzzleManiacs.Level;

namespace PuzzleManiacs.HUD
{
    /// <summary>
    /// Ÿ��Ʋ �ΰ�, ������ �ΰ�� ���� UI�� �����ϴ� Ŭ����.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TitleScreen : OpenToCloseTransition
    {
        #region Options
        /// <summary>
        /// Title Screen�� ���� BGM.
        /// </summary>
        [Header("Options")]
        [Tooltip("Title Screen�� ���� BGM.")]
        [SerializeField] private BGMLoopData titleTheme;

        /// <summary>
        /// ���� ����, ���� ����, ���� ����� ���� �޴����� �����ϴ� �ʱ� Menu System.
        /// </summary>
        [Tooltip("���� ����, ���� ����, ���� ����� ���� �޴����� �����ϴ� �ʱ� Menu System.")]
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