using UnityEngine;

public class TridentOfPoseidon : PassiveAuraSkillBase
{
    [Header("이속 감소 범위")]
    [SerializeField]
    private float _passiveRange = 1.0f;

    [SerializeField] private SlowZone _slowZone;
}