﻿using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Validators;
using DarkBestiary.Values;
using UnityEngine;

namespace DarkBestiary.Behaviours
{
    public class StatusEffectDamageBehaviour : DamageBehaviour
    {
        private readonly StatusEffectDamageBehaviourData data;

        public StatusEffectDamageBehaviour(StatusEffectDamageBehaviourData data,
            List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            this.data = data;
        }

        protected override float OnGetDamageMultiplier(GameObject victim, GameObject attacker, ref Damage damage)
        {
            var statusFlags = victim.GetComponent<BehavioursComponent>().Behaviours.Aggregate(StatusFlags.None, (current, b) => current | b.StatusFlags);

            if ((statusFlags & this.data.DamageStatusFlags) == 0)
            {
                return 0;
            }

            return this.data.Amount * StackCount;
        }
    }
}