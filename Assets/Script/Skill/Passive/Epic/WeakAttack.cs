using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAttack : PassiveSkillBase
{
    public override bool Activate(GameObject target)
    {
        if (!CheckTrigger()) return false;

        return false;
    }
}
