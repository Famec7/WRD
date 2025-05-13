using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적들 연출 관리하는 클래스
/// </summary>
public class MonsterEffecter : MonoBehaviour
{
    [SerializeField] SpriteRenderer monsterRenderer;//몬스터가 사용하는 메테리얼

    MaterialPropertyBlock materialPB;
    Material defaultMaterial;
    MaterialPropertyBlock defaultMaterialPB;

    [Header("Hit Effect Setting")]
    [SerializeField] float hitEffectDuration = 1f;
    [SerializeField] AnimationCurve hitEffectCurve;

    int hitValue = Shader.PropertyToID("_HitValue");

    [Header("Wound Effect Setting")]
    [SerializeField] GameObject woundEffect;

    [Header("Mark Effect Setting")]
    [SerializeField] GameObject markEffect;

    [Header("Slow Effect Setting")]
    [SerializeField] GameObject slowEffect;
    
    [Header("Joker Effect Setting")]
    [SerializeField] GameObject jokerEffect;
    
    [Header("Eyes Of Snake Effect Setting")]
    [SerializeField] GameObject eyesOfSnakeEffect;

    [Header("Monster Dead Effect Setting")]
    [SerializeField] string deadEffectName = "MonsterDead";

    //[Header("Debuff Effect Setting")]
    int debuffValue = Shader.PropertyToID("_GlowValue");

    // Start is called before the first frame update
    void Awake()
    {
        

        //부모 객체에 몬스터 스프라이트 컴포넌트가 있는지 확인
        monsterRenderer = GetComponentInParent<SpriteRenderer>();

        //부모 객체에 없으면 현재 객체에서 찾아보기
        if (monsterRenderer == null)
            if(TryGetComponent<SpriteRenderer>(out monsterRenderer) == false)
                Debug.LogError("MonsterEffecter가 Sprite Renderer 컴포넌트를 찾을 수 없음");

        //Material Property Block 생성하기
        materialPB = new MaterialPropertyBlock();
        defaultMaterialPB = new MaterialPropertyBlock();
        monsterRenderer.GetPropertyBlock(defaultMaterialPB);
        //defaultMaterial = monsterRenderer.sharedMaterial;

        InitEffects();

        Monster monster = GetComponentInParent<Monster>();
        if(monster == null)
            if(TryGetComponent<Monster>(out monster) == false)
                Debug.LogError("MonsterEffecter가 Monster 컴포넌트를 찾을 수 없음");

        //몬스터 Action에 필요한 연출 기능 추가
        monster.OnMonsterAttacked += StartHitEffect;
        monster.OnMonsterStart += InitEffects;
        monster.OnMonsterDeath += MonsterDeadEffect;
    }

    private void Update()
    {
        //히트 이펙트 테스트
        //if (Input.GetKeyDown(KeyCode.P)) StartHitEffect();
        //if (Input.GetKeyDown(KeyCode.O)) SetWoundEffect(true);
        //if (Input.GetKeyDown(KeyCode.I)) SetMarkEffect(true);
        //if (Input.GetKeyDown(KeyCode.U)) SetDebuffEffect(false);
        //if (Input.GetKeyDown(KeyCode.Y)) SetDebuffEffect(true);
    }

    /// <summary>
    /// 연출 초기화 함수
    /// </summary>
    void InitEffects()
    {
        //monsterRenderer.material.SetFloat(hitValue, 0f);
        monsterRenderer.SetPropertyBlock(defaultMaterialPB);

        SetWoundEffect(false);
        SetMarkEffect(false);
        SetSlowEffect(false);
        //SetDebuffEffect(false);
    }

    #region Set Default Material Setting
    int materialChangeStack;//메테리얼에서 변경된 값이 없으면 0, 있으면 1이상의 값을 가지게 됨

    void ChangeMaterial()
    {
        materialChangeStack += 1;

        //Debug.Log(materialChangeStack);
    }

    void UndoMaterialChange()
    {
        materialChangeStack -= 1;
        //Debug.Log(materialChangeStack);

        if (materialChangeStack == 0)
        {
            //monsterRenderer.material = defaultMaterial;
            //Debug.Log("초기화");
            //material property block 초기화
            //monsterRenderer.SetPropertyBlock(materialPB);
            //monsterRenderer.SetPropertyBlock(materialPB);

            monsterRenderer.SetPropertyBlock(defaultMaterialPB);
        }
    }


    #endregion

    #region Hit Effect

    Coroutine hitEffectCoroutine = null;

    /// <summary>
    /// 히트 이펙트 실행 함수
    /// </summary>
    public void StartHitEffect()
    {
        //예외처리: 몬스터 비활설화 일때 히트 이펙트 호출되는 경우 생기는듯
        if (this.gameObject.activeSelf == false) return;

        //이미 히트 이펙트가 진행되는게 있으면 꺼주기
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
            EndHitEffect();
        }

        //히트 이펙트 진행
        hitEffectCoroutine = StartCoroutine(HitEffect());
    }

    void EndHitEffect()
    {
        hitEffectCoroutine = null;//hitEffect 코루틴 변수 비워두기

        //히트 이펙트 값 초기화
        //monsterRenderer.material.SetFloat(hitValue, 0f);

        UndoMaterialChange();
    }

    IEnumerator HitEffect()
    {
        ChangeMaterial();//메테리얼 변경이 있다고 알림

        float _time = 0f;

        //히트 이펙트 애니메이션 진행
        while(_time < hitEffectDuration)
        {
            _time += Time.deltaTime;

            //Debug.Log(hitEffectCurve.Evaluate(_time / hitEffectDuration));

            //monsterRenderer.material.SetFloat(hitValue, hitEffectCurve.Evaluate(_time / hitEffectDuration));

            monsterRenderer.GetPropertyBlock(materialPB);
            materialPB.SetFloat(hitValue, hitEffectCurve.Evaluate(_time / hitEffectDuration));
            monsterRenderer.SetPropertyBlock(materialPB);

            yield return null;
        }

        EndHitEffect();

        //히트 이펙트 값 초기화
        //monsterRenderer.material.SetFloat(hitValue, 0f);

        //기존 메테리얼로 변경
        //monsterRenderer.material = defaultMaterial;
        
    }

    #endregion

    /// <summary>
    /// 자상 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetWoundEffect(bool on)
    {
        if (woundEffect == null) return;
        woundEffect.SetActive(on);
    }

    /// <summary>
    /// 표적 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetMarkEffect(bool on)
    {
        if (markEffect == null) return;
        markEffect.SetActive(on);
    }

    /// <summary>
    /// 느려지는 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetSlowEffect(bool on)
    {
        if (slowEffect == null) return;
        slowEffect.SetActive(on);
    }

    /// <summary>
    /// 조커 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetJokerEffect(bool on)
    {
        if (jokerEffect == null) return;
        
        jokerEffect.transform.position = monsterRenderer.bounds.center + new Vector3(0, monsterRenderer.bounds.extents.y, 0);
        jokerEffect.SetActive(on);

        if (jokerEffect.TryGetComponent(out Animator animator))
        {
            if (on)
                animator.Play("JokerMark");
        }
    }
    
    /// <summary>
    /// 뱀의 주시 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetSnakeEffect(bool on, float duration)
    {
        if (eyesOfSnakeEffect == null) return;

        eyesOfSnakeEffect.transform.position = monsterRenderer.bounds.center + new Vector3(0, monsterRenderer.bounds.extents.y, 0);
        eyesOfSnakeEffect.SetActive(on);
        
        if (on)
        {
            //뱀의 주시 이펙트가 켜지면 n초 후에 꺼지게 하기
            StartCoroutine(IE_SetSnakeEffect(duration));
        }
        else
        {
            eyesOfSnakeEffect.SetActive(false);
        }
    }
    
    private IEnumerator IE_SetSnakeEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetSnakeEffect(false, 0);
    }

    /// <summary>
    /// 디버프 연출 사용 함수
    /// </summary>
    /// <param name="on"></param>
    public void SetDebuffEffect(bool on)
    {
        float value = on ? 1 : 0;

        //monsterRenderer.material.SetFloat(debuffValue, value);
        

        if (on)
        {
            //monsterRenderer.SetPropertyBlock(materialPB);
            if (materialPB.GetFloat(debuffValue) != value) ChangeMaterial();//메테리얼 변경했다고 알림

            //monsterRenderer.material.SetFloat(debuffValue, value);

            monsterRenderer.GetPropertyBlock(materialPB);
            materialPB.SetFloat(debuffValue, value);
            monsterRenderer.SetPropertyBlock(materialPB);
        }
        else
        {

            if (materialPB.GetFloat(debuffValue) != value)
            {
                monsterRenderer.GetPropertyBlock(materialPB);
                materialPB.SetFloat(debuffValue, value);
                monsterRenderer.SetPropertyBlock(materialPB);

                //monsterRenderer.material.SetFloat(debuffValue, value);
                UndoMaterialChange();
            }
            //monsterRenderer.SetPropertyBlock(null);
        }
    }

    public void MonsterDeadEffect()
    {
        //몬스터 사망 파티클 생성
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>(deadEffectName);

        effect.gameObject.transform.position = transform.position;
    }
}
