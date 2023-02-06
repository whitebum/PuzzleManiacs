using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PuzzleExpress.HUD;

namespace PuzzleExpress.FlipCards.HUD
{
    /// <summary>
    /// Text 객체를 통하여 스코어를 표기하는 UI.
    /// </summary>
    public class TextScoreViewer : MonoBehaviour, IBaseScoreViewer
    {
        /// <summary>
        /// 스코어를 표기하는 Text 객체.
        /// </summary>
        [Tooltip("스코어를 표기하는 Text 객체.")]
        [SerializeField] private Text viewer;

        /// <summary>
        /// 스코어를 표기할 방식.
        /// </summary>
        [Tooltip("스코어를 표기할 방식.")]
        [SerializeField] private string displayFormat;

        private void Reset()
        {
            viewer = GetComponentsInChildren<Text>()[1];

            displayFormat = "{0:#,0}";
        }

        private void Awake()
        {
            viewer = viewer ?? GetComponentsInChildren<Text>()[1];
        }

        public void Display(int score)
        {
            if (viewer != null)
            {
                viewer.text = score.ToString(displayFormat);
            }
        }
    }
}
