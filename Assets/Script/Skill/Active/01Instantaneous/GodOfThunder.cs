    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GodOfThunder : InstantaneousSkill
    {
        private int _originPassiveChance;
        private float _originAttackRange;
        
        [SerializeField]
        private float _chainAttackRange = 7.0f;
        
        private ParticleEffect _electricAura = null;
        
        #region Data

        private float _duration;
        private float _range;
        private float _damage;
        private int _passiveChance;
        private float _stunDuration;

        #endregion
        
        public override void OnActiveEnter()
        {
            _originPassiveChance = weapon.GetPassiveSkill().Data.Chance;
            _originAttackRange = weapon.Data.AttackRange;
            
            _duration = Data.GetValue(0);
            _range = Data.GetValue(1);
            _passiveChance = (int)Data.GetValue(2);
            _damage = Data.GetValue(3);
            _stunDuration = Data.GetValue(4);
            
            // 패시브 스킬 확률을 변경
            weapon.GetPassiveSkill().Data.Chance = _passiveChance;
            
            // 무기의 공격 범위를 변경
            weapon.Data.AttackRange = _range;
            
            _electricAura = EffectManager.Instance.CreateEffect<ParticleEffect>("ElectricAura");
            _electricAura.SetPosition(weapon.owner.transform.position);
            _electricAura.transform.localPosition = Vector3.zero;
            _electricAura.transform.SetParent(weapon.owner.transform, false);
            
            _electricAura.PlayEffect();
            
            weapon.AddAction(ChainAttack);
        }

        public override bool OnActiveExecute()
        {
            _duration -= Time.deltaTime;
            
            return _duration <= 0;
        }

        public override void OnActiveExit()
        {
            ResetStat();
            
            if(_electricAura is null)
                return;
            
            _electricAura.StopEffect();
            _electricAura = null;
            
            weapon.RemoveAction(ChainAttack);
        }

        private void ResetStat()
        {
            // 패시브 스킬 확률을 원래대로 변경
            weapon.GetPassiveSkill().Data.Chance = _originPassiveChance;
            
            // 무기의 공격 범위를 원래대로 변경
            weapon.Data.AttackRange = _originAttackRange;
        }

        #region ChainAttack

        private void ChainAttack()
        {
            Vector3 pos = weapon.owner.Target.transform.position;
            var targets = RangeDetectionUtility.GetAttackTargets(pos, _chainAttackRange, default, targetLayer);

            StopCoroutine(IE_ChainAttack(targets));
        }

        private IEnumerator IE_ChainAttack(List<Collider2D> targets)
        {
            for(int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                
                if (target.TryGetComponent(out Status status))
                {
                    if(status.ElectricShockStack == 0)
                        continue;
                    
                    target.GetComponent<Monster>().HasAttacked(_damage);
                    
                    StatusEffectManager.Instance.RemoveStatusEffect(status, typeof(ElectricShock));
                    StatusEffectManager.Instance.AddStatusEffect(status, new Stun(status.gameObject, _stunDuration));

                    if (i + 1 < targets.Count)
                    {
                        Vector3 from = target.transform.position;
                        Vector3 to = targets[i + 1].transform.position;
                        CreateLightningEffect(from, to);
                    }
                    
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        private void CreateLightningEffect(Vector3 from, Vector3 to)
        {
            
        }

        #endregion
    }