using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaSpriteChanger : MonoBehaviour
{
    [System.Serializable]
    class MatryoshkaSprite
    {
        public Sprite _2_insideSprite1;
        public Sprite _4_insideSprite2;

        [Space(10f)]
        public Sprite _3_coreSprite;

        [Space(10f)]
        public Sprite _1_outsideSprite1;
        public Sprite _5_outsideSprite2;
    }

    [Header("Sprite Renderers")]
    [SerializeField] SpriteRenderer _2_insideSprite1Renderer;
    [SerializeField] SpriteRenderer _4_insideSprite2Renderer;

    [Space(5f)]
    [SerializeField] SpriteRenderer _3_coreSpriteRenderer;

    [Space(5f)]
    [SerializeField] SpriteRenderer _1_outsideSprite1Renderer;
    [SerializeField] SpriteRenderer _5_outsideSprite2Renderer;

    [Header("Each Level Sprite")]
    [SerializeField] MatryoshkaSprite sprite_level1;

    [Space(5f)]
    [SerializeField] MatryoshkaSprite sprite_level2;

    [Space(5f)]
    [SerializeField] MatryoshkaSprite sprite_level3;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeMatryoshikaSprite(Random.Range(1, 4));
        }
#endif
    }

    /// <summary>
    /// 마트료시카의 스프라이트를 변경해주는 함수
    /// </summary>
    /// <param name="level">레벨은 1,2,3 으로 3개 존재</param>
    public void ChangeMatryoshikaSprite(int level)
    {
        MatryoshkaSprite levelSprite;
        //레벨 선택
        switch (level)
        {
            case 1:
                levelSprite = sprite_level1;
                break;

            case 2: 
                levelSprite = sprite_level2;
                break;

            case 3:
                levelSprite = sprite_level3;
                break;

            default:
                Debug.LogError("마트료시카에 없는 레벨의 스프라이트를 찾고 있음");
                levelSprite = sprite_level1;//에러가 없기 위해 우선 level1로 변경
                break;
        }

        _2_insideSprite1Renderer.sprite = levelSprite._2_insideSprite1;
        _4_insideSprite2Renderer.sprite = levelSprite._4_insideSprite2;

        _3_coreSpriteRenderer.sprite = levelSprite._3_coreSprite;

        _1_outsideSprite1Renderer.sprite = levelSprite._1_outsideSprite1;
        _5_outsideSprite2Renderer.sprite = levelSprite._5_outsideSprite2;
    }
}
