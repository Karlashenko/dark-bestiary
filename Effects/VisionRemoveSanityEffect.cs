using System.Collections.Generic;
using DarkBestiary.Data;
using DarkBestiary.Validators;
using DarkBestiary.Visions;
using UnityEngine;

namespace DarkBestiary.Effects
{
    public class VisionRemoveSanityEffect : Effect
    {
        private readonly VisionSanityEffectData data;

        public VisionRemoveSanityEffect(VisionSanityEffectData data, List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            this.data = data;
        }

        protected override Effect New()
        {
            return new VisionRemoveSanityEffect(this.data, this.Validators);
        }

        protected override void Apply(GameObject caster, GameObject target)
        {
            Apply(caster, target.transform.position);
        }

        protected override void Apply(GameObject caster, Vector3 target)
        {
            if (VisionManager.Instance == null)
            {
                TriggerFinished();
                return;
            }

            VisionManager.Instance.Sanity -= this.data.Amount;
            TriggerFinished();
        }
    }
}