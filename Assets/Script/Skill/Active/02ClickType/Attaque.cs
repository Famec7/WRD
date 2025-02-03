using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attaque : ClickTypeSkill
{
    #region Data

    private float _duration;
    private float _damage;
    private int _passiveChance;
    private int _originPassiveChance;
    #endregion

    public float MaxDashDistance = 2f;
    public float InitialSpeed = 3f;       
    public float MaxSpeed = 5f;
    
    private Coroutine _dashCoroutine = null;
    protected override void Init()
    {
        base.Init();

        _damage = Data.GetValue(0);
        _duration = Data.GetValue(1);
        _passiveChance = (int)Data.GetValue(2);
    }

    public override void OnActiveEnter()
    {
        _originPassiveChance = weapon.GetPassiveSkill().Data.Chance;

        float distacne = Mathf.Clamp(Vector2.Distance(ClickPosition, (Vector2)weapon.owner.transform.position),0, MaxDashDistance);
        Vector2 direction = (ClickPosition - (Vector2)weapon.owner.transform.position).normalized;
        Vector2 targetPosition = (Vector2)weapon.owner.transform.position + (direction * distacne);
        
        weapon.owner.GetComponent<PlayerController>().enabled = false;

        if (_dashCoroutine != null)
        {
            StopCoroutine(_dashCoroutine);
        }

        _dashCoroutine = StartCoroutine(Dash(targetPosition));
        var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, targetLayer);

        if (targets.Count == 0)
            return;

        foreach (var tar in targets)
        {
            if (tar.TryGetComponent(out Monster monster))
            {
                monster.HasAttacked(_damage);
                if (monster.isDead)
                    StartCoroutine(BoostPassiveChance());

                break;
            }
        }

        weapon.owner.GetComponent<PlayerController>().enabled = true;
    }

    public override bool OnActiveExecute()
    {
        return true;
    }

    public override void OnActiveExit()
    {
        ;
    }

    private IEnumerator Dash(Vector3 targetPosition)
    {
        float currentSpeed = InitialSpeed;

        while (Vector3.Distance(weapon.owner.transform.position, targetPosition) > 0.1f)
        {
            currentSpeed = Mathf.Min(currentSpeed + 0.5f * Time.deltaTime, MaxSpeed);
            weapon.owner.transform.position = Vector2.MoveTowards(weapon.owner.transform.position, targetPosition, currentSpeed * Time.deltaTime);
            yield return null;
        }

        weapon.owner.transform.position = targetPosition;

       
    }

    private IEnumerator BoostPassiveChance()
    {
        weapon.GetPassiveSkill().Data.Chance = _passiveChance;
        yield return new WaitForSeconds(_duration);
        weapon.GetPassiveSkill().Data.Chance = _originPassiveChance;
    }
}
