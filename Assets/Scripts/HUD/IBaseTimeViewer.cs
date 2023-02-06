using System;

namespace PuzzleExpress.HUD
{
    /// <summary>
    /// UI�� �ð� ǥ�⸦ �����ϴ� �������̽�.
    /// </summary>
    public interface IBaseTimeViewer
    {
        /// <summary>
        /// �ð��� ǥ���մϴ�.
        /// </summary>
        /// <param name="time">ǥ���� �ð�.</param>
        void Display(TimeSpan time, float maxTime);       
    }
}