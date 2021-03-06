﻿using DarkBestiary.Attributes;
using DarkBestiary.Components;
using DarkBestiary.Data;
using UnityEngine;
using Attribute = DarkBestiary.Attributes.Attribute;

namespace DarkBestiary.Modifiers
{
    public class AttributeModifier : FloatModifier
    {
        public GameObject Entity { get; set; }

        public Attribute Attribute { get; }
        public AttributeModifierData Data { get; }

        private readonly AttributeModifierData data;

        public AttributeModifier(Attribute attribute, AttributeModifierData data) : base(data.Amount, data.Type)
        {
            Data = data;
            Attribute = attribute;
        }

        public override string ToString()
        {
            return $"+{GetAmount()} {Attribute.Name}";
        }

        public override float GetAmount()
        {
            return Type == ModifierType.Flat ? Mathf.Floor(base.GetAmount()) : base.GetAmount();
        }

        public override float Modify(float value)
        {
            value = base.Modify(value);

            var attributes = Entity.GetComponent<AttributesComponent>();

            if (Data.MaxAttributeFraction > 0)
            {
                value += attributes.GetMaxAttribute() * (Data.MaxAttributeFraction * StackCount);
            }

            if (Data.AttributeFraction.AttributeType == AttributeType.None)
            {
                return value;
            }

            value += attributes.Get(Data.AttributeFraction.AttributeType).Value() * (Data.AttributeFraction.Fraction * StackCount);

            return value;
        }
    }
}