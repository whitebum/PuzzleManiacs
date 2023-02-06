namespace PuzzleExpress.HUD
{
    /// <summary>
    /// UI의 스코어 표기를 구현하는 인터페이스.
    /// </summary>
    public interface IBaseScoreViewer
    {
        void Display(int score);
    }
}
