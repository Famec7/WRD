using UnityEngine;

public class BombNumController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bombSpriteRenderer; 
    [SerializeField] private Sprite[] bombSprites;

    public int BombCount { get; private set; }

    public Transform target; // 몬스터(부모)의 Transform 참조

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 0.2f, 0);
            transform.rotation = Quaternion.identity;
        }
    }

    public void UpdateBombCount(int delta)
    {
        BombCount += delta;

        if (BombCount > 0 && BombCount <= bombSprites.Length)
        {
            bombSpriteRenderer.sprite = bombSprites[BombCount - 1];
        }
        else if (BombCount <= 0)
        {
            BombCount = 0;
            bombSpriteRenderer.sprite = null; 
        }
    }

}
