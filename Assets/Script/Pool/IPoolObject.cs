/// <summary>
/// 오브젝트 생성/삭제 시 GetFromPool, ReturnToPool 함수를 자동으로 호출
/// </summary>
public interface IPoolObject
{
    void GetFromPool();
    void ReturnToPool();
}