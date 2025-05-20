using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushVariation : PassiveSkillBase
{
    #region Data

    private float _originAngleZ;
    private float angleZ;

    private Vector2 _originPosition;
    #endregion

    public float DashDistance = 2f;
    public float InitialSpeed = 3f;
    public float MaxSpeed = 5f;

    private Coroutine _dashCoroutine = null;

    public Vector3 pivot = new Vector3(-0.181f, -0.1f, 0);
    
    [SerializeField]
    private AudioClip dashSound;

    public override bool Activate(GameObject target = null)
    {
        if (!CheckTrigger() || target == null) return false;
        SoundManager.Instance.PlaySFX(dashSound);
        StartCoroutine(IE_Activate(target));
        return true;
    }

    private IEnumerator IE_Activate(GameObject target)
    {

        Vector2 direction = ((Vector2)target.transform.position - (Vector2)weapon.transform.position).normalized;
        Vector2 targetPosition = (Vector2)weapon.transform.position + (direction);

        _originAngleZ = transform.rotation.eulerAngles.z;
        angleZ = Vector3.SignedAngle(transform.up, direction, transform.forward);
        weapon.gameObject.transform.localRotation = Quaternion.Euler(0, 0, angleZ + _originAngleZ - 90f);

        _originPosition = weapon.owner.transform.position;

        weapon.owner.GetComponent<PlayerController>().enabled = false;
        weapon.enabled = false;

        if (_dashCoroutine != null)
        {

            StopCoroutine(_dashCoroutine);
        }

        _dashCoroutine = StartCoroutine(Dash(targetPosition));


        yield return new WaitForSeconds(Data.GetValue(0));
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
            var targets = RangeDetectionUtility.GetAttackTargets(targetPosition, 1.5f, default, targetLayer);

            if (targets.Count == 0)
                yield return null;

            foreach (var tar in targets)
            {
                if (tar.TryGetComponent(out Monster monster))
                {
                    monster.HasAttacked(Data.GetValue(1));
                    monster.HasAttackedPercent(Data.GetValue(2));
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
