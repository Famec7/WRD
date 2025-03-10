using UnityEngine;

public class BombNumController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bombSpriteRenderer;
    [SerializeField] private Sprite[] bombSprites;
    public int BombCount { get; private set; }

    public Transform target; // 몬스터(부모)의 Transform 참조

    private void LateUpdate()
    {
        // 대상이 존재하면 위치 및 회전 고정
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 0.2f, 0);
            transform.rotation = Quaternion.identity;
        }

        // 무기가 더 이상 착용중이지 않으면 자동 삭제
        if (!WeaponManager.Instance.IsEuqiped(410))
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBombCount(int delta)
    {
        BombCount += delta;

        if (BombCount > 0 && BombCount <= bombSprites.Length)
        {
            bombSpriteRenderer.sprite = bombSprites[BombCount - 1];
        }
        else // BombCount가 0 이하이거나 범위를 벗어나면 초기화
        {
            BombCount = 0;
            bombSpriteRenderer.sprite = null;
        }
    }
}
