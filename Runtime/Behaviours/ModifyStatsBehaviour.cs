﻿using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Attributes;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Data.Repositories;
using DarkBestiary.Modifiers;
using DarkBestiary.Properties;
using DarkBestiary.Validators;
using UnityEngine;

namespace DarkBestiary.Behaviours
{
    public class ModifyStatsBehaviour : Behaviour
    {
        private readonly List<AttributeModifier> m_AttributeModifiers = new();
        private readonly List<PropertyModifier> m_PropertyModifiers = new();

        public ModifyStatsBehaviour(
            ModifyStatsBehaviourData data,
            IAttributeRepository attributeRepository,
            IPropertyRepository propertyRepository,
            List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            foreach (var attributeModifierData in data.AttributeModifiers)
            {
                var modifier = new AttributeModifier(
                    attributeRepository.FindOrFail(attributeModifierData.AttributeId),
                    attributeModifierData
                );

                m_AttributeModifiers.Add(modifier);
            }

            foreach (var propertyModifierData in data.PropertyModifiers)
            {
                var modifier = new PropertyModifier(
                    propertyRepository.FindOrFail(propertyModifierData.PropertyId),
                    propertyModifierData
                );

                m_PropertyModifiers.Add(modifier);
            }
        }

        protected override void OnApply(GameObject caster, GameObject target)
        {
            target.GetComponent<AttributesComponent>().ApplyModifiers(m_AttributeModifiers);
            target.GetComponent<PropertiesComponent>().ApplyModifiers(m_PropertyModifiers);
        }

        protected override void OnRemoved(GameObject source, GameObject target)
        {
            target.GetComponent<AttributesComponent>().RemoveModifiers(m_AttributeModifiers);
            target.GetComponent<PropertiesComponent>().RemoveModifiers(m_PropertyModifiers);
        }

        protected override void OnStackCountChanged(Behaviour _, int delta)
        {
            foreach (var modifier in m_AttributeModifiers)
            {
                modifier.ChangeStack(StackCount);
            }

            foreach (var modifier in m_PropertyModifiers)
            {
                modifier.ChangeStack(StackCount);
            }
        }

        public string GetDefenseString(GameObject entity)
        {
            var modifier = m_AttributeModifiers.First(m => m.Attribute.Type == AttributeType.Defense);

            return AttributeModifierValueString(modifier);
        }

        public string GetResistanceString(GameObject entity)
        {
            var modifier = m_AttributeModifiers.First(m => m.Attribute.Type == AttributeType.Resistance);

            return AttributeModifierValueString(modifier);
        }

        public string GetMightString(GameObject entity)
        {
            var modifier = m_AttributeModifiers.First(m => m.Attribute.Type == AttributeType.Might);

            return AttributeModifierValueString(modifier);
        }

        public string GetConstitutionString(GameObject entity)
        {
            var modifier = m_AttributeModifiers.First(m => m.Attribute.Type == AttributeType.Constitution);

            return AttributeModifierValueString(modifier);
        }

        public string GetLeadershipString(GameObject entity)
        {
            var modifier = m_AttributeModifiers.First(m => m.Attribute.Type == AttributeType.Leadership);

            return AttributeModifierValueString(modifier);
        }

        public string GetMinionHealthString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.MinionHealth);

            return PropertyModifierValueString(modifier);
        }

        public string GetMinionDamageString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.MinionDamage);

            return PropertyModifierValueString(modifier);
        }

        public string GetDamageString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.DamageIncrease);

            return PropertyModifierValueString(modifier);
        }

        public string GetDamageReductionString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.IncomingDamageReduction);

            return PropertyModifierValueString(modifier);
        }

        public string GetDamageReflectionString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.DamageReflection);

            return PropertyModifierValueString(modifier);
        }

        public string GetPhysicalDamageReductionString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.IncomingPhysicalDamageReduction);

            return PropertyModifierValueString(modifier);
        }

        public string GetMagicalDamageReductionString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.IncomingMagicalDamageReduction);

            return PropertyModifierValueString(modifier);
        }

        public string GetAbsDamageString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.DamageIncrease);

            return PropertyModifierValueString(modifier, true);
        }

        public string GetHealthRegenerationString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.HealthRegeneration);

            return PropertyModifierValueString(modifier);
        }

        public string GetThornsString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Thorns);

            return PropertyModifierValueString(modifier);
        }

        public string GetExperienceString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Experience);

            return PropertyModifierValueString(modifier);
        }

        public string GetCriticalChanceString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.CriticalHitChance);

            return PropertyModifierValueString(modifier);
        }

        public string GetHealingCriticalChanceString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.HealingCriticalChance);

            return PropertyModifierValueString(modifier);
        }

        public string GetCriticalDamageString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.CriticalHitDamage);

            return PropertyModifierValueString(modifier);
        }

        public string GetRageGenerationString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.RageGeneration);

            return PropertyModifierValueString(modifier);
        }

        public string GetVampirismString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Vampirism);

            return PropertyModifierValueString(modifier);
        }

        public string GetHealingString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.HealingIncrease);

            return PropertyModifierValueString(modifier);
        }

        public string GetDodgeChanceString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Dodge);

            return PropertyModifierValueString(modifier);
        }

        public string GetBlockChanceString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.BlockChance);

            return PropertyModifierValueString(modifier);
        }

        public string GetIncomingHealingString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.IncomingHealingIncrease);

            return PropertyModifierValueString(modifier);
        }

        public string GetAbsIncomingHealingString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.IncomingHealingIncrease);

            return PropertyModifierValueString(modifier, true);
        }

        public string GetHealthString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Health);

            return PropertyModifierValueString(modifier);
        }

        public string GetAbsHealthString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.Health);

            return PropertyModifierValueString(modifier, true);
        }

        public string GetMagicPenetrationString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.MagicPenetration);

            return PropertyModifierValueString(modifier);
        }

        public string GetArmorPenetrationString(GameObject entity)
        {
            var modifier = m_PropertyModifiers.First(m => m.Property.Type == PropertyType.ArmorPenetration);

            return PropertyModifierValueString(modifier);
        }

        private string AttributeModifierValueString(AttributeModifier modifier)
        {
            modifier.Entity = modifier.Entity ?? Game.Instance.Character.Entity;
            modifier.ChangeStack(StackCount);

            return ((int) modifier.GetAmount()).ToString();
        }

        private string PropertyModifierValueString(PropertyModifier modifier, bool absolute = false)
        {
            modifier.Entity = modifier.Entity ?? Game.Instance.Character.Entity;
            modifier.ChangeStack(StackCount);

            var property = modifier.Entity.GetComponent<PropertiesComponent>().Get(modifier.Property.Id);
            var contains = property.Modifiers.Contains(modifier);

            if (contains)
            {
                property.Modifiers.Remove(modifier);
            }

            var value = property.Value();
            var delta = modifier.Modify(value) - value;

            delta = absolute ? Mathf.Abs(delta) : delta;

            if (contains)
            {
                property.Modifiers.Add(modifier);
            }

            return Property.ValueString(modifier.Property.Type, delta);
        }
    }
}