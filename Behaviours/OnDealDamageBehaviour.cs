﻿using System.Collections.Generic;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Data.Repositories;
using DarkBestiary.Effects;
using DarkBestiary.Extensions;
using DarkBestiary.Messaging;
using DarkBestiary.Validators;
using UnityEngine;

namespace DarkBestiary.Behaviours
{
    public class OnDealDamageBehaviour : Behaviour
    {
        private readonly OnDealDamageBehaviourData data;
        private readonly Effect effect;

        public OnDealDamageBehaviour(OnDealDamageBehaviourData data,
            List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            this.data = data;
            this.effect = Container.Instance.Resolve<IEffectRepository>().FindOrFail(data.EffectId);
        }

        protected override void OnApply(GameObject caster, GameObject target)
        {
            HealthComponent.AnyEntityDamaged += OnEntityDamaged;
        }

        protected override void OnRemoved(GameObject source, GameObject target)
        {
            HealthComponent.AnyEntityDamaged -= OnEntityDamaged;
        }

        private void OnEntityDamaged(EntityDamagedEventData data)
        {
            if (data.Damage.Amount < 1 && Mathf.Approximately(0, data.Damage.Absorbed) || data.Attacker == data.Victim)
            {
                return;
            }

            if (data.Attacker != Target ||
                this.data.DamageFlags != DamageFlags.None &&
                (data.Damage.Flags & this.data.DamageFlags) == 0 ||
                this.data.DamageInfoFlags != DamageInfoFlags.None &&
                (data.Damage.InfoFlags & this.data.DamageInfoFlags) == 0)
            {
                return;
            }

            if ((this.data.ExcludeDamageFlags & data.Damage.Flags) > 0 ||
                (this.data.ExcludeDamageInfoFlags & data.Damage.InfoFlags) > 0)
            {
                return;
            }

            if (this.data.DamageTypes.Count > 0 && !this.data.DamageTypes.Contains(data.Damage.Type))
            {
                return;
            }

            if (!this.Validators.ByPurpose(ValidatorPurpose.Other).Validate(data.Attacker, data.Victim))
            {
                return;
            }

            var clone = this.effect.Clone();
            clone.StackCount = StackCount;

            // Note: if this effect will apply behaviour with BreaksOnTakeDamage flag, it will be removed immediately.
            Timer.Instance.WaitForFixedUpdate(() =>
            {
                // Wait for BehavioursComponent OnTakeDamage callback...
                Timer.Instance.WaitForFixedUpdate(() =>
                {
                    clone.Apply(Caster, EventSubject == BehaviourEventSubject.Me ? Target : data.Victim);
                });
            });
        }
    }
}