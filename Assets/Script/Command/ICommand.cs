public interface ICommand
{
    /// <summary>
    /// 명령을 실행
    /// </summary>
    /// <returns> 명령이 성공적으로 실행되었는지 여부 (ture: 실행 끝 / false: 계속 실행) </returns>
    bool Execute();
    
    /// <summary>
    /// 명령을 취소
    /// </summary>
    void Undo();
    
    /// <summary>
    /// 명령이 완료되었을 때 호출
    /// </summary>
    void OnComplete();
}