using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : GuidedProjectile
{
    public bool IsAttached = false;
    public float damage;

    protected override void MoveToTarget()
    {
        if (Target is null || !Target.activeSelf || IsAttached)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance < 0.1f)
        {
            IsAttached = true;

            if (Target != null)
            {
                transform.SetParent(Target.transform);
                transform.localPosition = new Vector3(0, 0, 0);
            }
            else
                ReturnToPool();
        }


        float timeSinceStart = Time.time - _startTime;
        float fractionOfJourney = timeSinceStart / _journeyLength;
        float easedFraction = curve.Evaluate(fractionOfJourney);

        transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, easedFraction);
    }

    public void Explosion()
    {
        if (Target is null)
        {
            ReturnToPool();
            return;
        }

        Target.GetComponent<Monster>().HasAttacked(damage);
        ParticleEffect effect = EffectManager.Instance.CreateEffect<ParticleEffect>("FireEffect");
        effect.SetPosition(transform.position);
    }
}
