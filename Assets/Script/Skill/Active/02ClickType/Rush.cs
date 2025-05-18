using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rush : ClickTypeSkill
{
    #region Data

    private float _duration;
    private float _damage;
    private int _reducedSpeed;
    private float _originAngleZ;
    private float angleZ;

    private Vector2 _originPosition;
    #endregion

    public float DashDistance = 2f;
    public float InitialSpeed = 3f;
    public float MaxSpeed = 5f;

    public Vector3 pivot = new Vector3(-0.181f, -0.1f,0);

    private Coroutine _dashCoroutine = null;

    public override void OnActiveEnter()
    {
        _damage = Data.GetValue(0);
        _duration = Data.GetValue(1);
        _reducedSpeed = (int)Data.GetValue(2);
    }

    public override bool OnActiveExecute()
    {
        Vector2 direction = (ClickPosition - (Vector2)weapon.transform.position).normalized;
        Vector2 targetPosition = (Vector2)weapon.transform.position + (direction);

        _originAngleZ = transform.rotation.eulerAngles.z;
        angleZ = Vector3.SignedAngle(transform.up, direction, transform.forward);
        weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angleZ+_originAngleZ - 90f);

        _originPosition = weapon.owner.transform.position;

        weapon.owner.GetComponent<PlayerController>().enabled = false;
        weapon.enabled = false;

        if (_dashCoroutine != null)
        {
            
            StopCoroutine(_dashCoroutine);
        }

        _dashCoroutine = StartCoroutine(Dash(targetPosition));
        
        return true;
    }

    public override void OnActiveExit()
    {
    }

    private IEnumerator Dash(Vector3 targetPosition)
    {
        float currentSpeed = InitialSpeed;

        while (Vector3.Distance(weapon.transform.position, targetPosition) > 0.1f)
        {
            currentSpeed = Mathf.Min(currentSpeed + 0.5f * Time.deltaTime, MaxSpeed);
            weapon.transform.position = Vector2.MoveTowards(weapon.transform.position, targetPosition, currentSpeed * Time.deltaTime);
            yield return null;
        }

        if (!weapon.enabled)
        {
            var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, Data.Range, default, targetLayer);

            if (targets.Count == 0)
                yield return null;

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Monster monster))
                {
                    monster.HasAttacked(_damage);
                    StatusEffectManager.Instance.AddStatusEffect(monster.status, new SlowDown(monster.status.gameObject, _reducedSpeed, _duration));
                    StatusEffectManager.Instance.AddStatusEffect(monster.status, new Wound(monster.status.gameObject));
                }
            }

            weapon.enabled = true;
            yield return new WaitForSeconds(0.3f);
            weapon.transform.localPosition = pivot;
            weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            weapon.owner.GetComponent<PlayerController>().enabled = true;
        }

        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("HeavyBlowEffect");
        effect.SetPosition(targetPosition);
        effect.PlayEffect();
    }

    void LateUpdate()
    {
        if (!weapon.enabled)
        {
            weapon.owner.GetComponent<PlayerController>().enabled = false;
            weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angleZ + _originAngleZ - 90f);
        }
    }

}

