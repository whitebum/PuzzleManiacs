using System;

namespace PuzzleExpress.HUD
{
    /// <summary>
    /// UI의 시간 표기를 구현하는 인터페이스.
    /// </summary>
    public interface IBaseTimeViewer
    {
        /// <summary>
        /// 시간을 표기합니다.
        /// </summary>
        /// <param name="time">표기할 시간.</param>
        void Display(TimeSpan time, float maxTime);       
    }
}