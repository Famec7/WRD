using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적들 연출 관리하는 클래스
/// </summary>
public class MonsterEffecter : MonoBehaviour
{
    [SerializeField] SpriteRenderer monsterRenderer;//몬스터가 사용하는 메테리얼

    [Header("Hit Effect Setting")]
    [SerializeField] float hitEffectDuration = 1f;
    [SerializeField] AnimationCurve hitEffectCurve;

    int hitValue = Shader.PropertyToID("_HitValue");

    // Start is called before the first frame update
    void Awake()
    {
        //부모 객체에 몬스터 스프라이트 컴포넌트가 있는지 확인
        monsterRenderer = GetComponentInParent<SpriteRenderer>();

        //부모 객체에 없으면 현재 객체에서 찾아보기
        if (monsterRenderer == null)
            if(TryGetComponent<SpriteRenderer>(out monsterRenderer) == false)
                Debug.LogError("MonsterEffecter가 Sprite Renderer 컴포넌트를 찾을 수 없음");

        InitEffects();
    }

    private void Update()
    {
        //히트 이펙트 테스트
        if (Input.GetKeyDown(KeyCode.P)) StartHitEffect();
    }

    /// <summary>
    /// 연출 초기화 함수
    /// </summary>
    void InitEffects()
    {
        monsterRenderer.material.SetFloat(hitValue, 0f);
    }

    #region Hit Effect

    Coroutine hitEffectCoroutine = null;

    /// <summary>
    /// 히트 이펙트 실행 함수
    /// </summary>
    public void StartHitEffect()
    {
        //이미 히트 이펙트가 진행되는게 있으면 꺼주기
        if (hitEffectCoroutine != null) StopCoroutine(hitEffectCoroutine);

        //히트 이펙트 진행
        hitEffectCoroutine = StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        float _time = 0f;

        //히트 이펙트 애니메이션 진행
        while(_time < hitEffectDuration)
        {
            _time += Time.deltaTime;

            monsterRenderer.material.SetFloat(hitValue, hitEffectCurve.Evaluate(_time / hitEffectDuration));

            yield return null;
        }

        //히트 이펙트 값 초기화
        monsterRenderer.material.SetFloat(hitValue, 0f);
    }

    #endregion
}
