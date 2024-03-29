using System.Collections.Generic;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Validators;
using DarkBestiary.Values;
using UnityEngine;

namespace DarkBestiary.Behaviours
{
    public class BackstabDamageBehaviour : DamageBehaviour
    {
        private readonly BackstabDamageBehaviourData m_Data;

        public BackstabDamageBehaviour(BackstabDamageBehaviourData data, List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            m_Data = data;
        }

        protected override float OnGetDamageMultiplier(GameObject victim, GameObject attacker, ref Damage damage)
        {
            if (victim.GetComponent<ActorComponent>().Model.IsFacingLeft !=
                attacker.GetComponent<ActorComponent>().Model.IsFacingLeft)
            {
                return 0;
            }

            damage.InfoFlags |= DamageInfoFlags.Backstab;

            return m_Data.Amount;
        }
    }
}