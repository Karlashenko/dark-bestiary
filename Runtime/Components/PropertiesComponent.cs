﻿using System;
using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Modifiers;
using DarkBestiary.Properties;
using UnityEngine;

namespace DarkBestiary.Components
{
    public class PropertiesComponent : Component
    {
        public event Action<PropertiesComponent, Property> PropertyChanged;

        public Dictionary<PropertyType, Property> Properties { get; private set; }

        private HealthComponent m_Health;

        public PropertiesComponent Construct(IEnumerable<Property> properties)
        {
            Properties = new Dictionary<PropertyType, Property>();

            foreach (var property in properties)
            {
                Properties.Add(property.Type, property);
            }

            return this;
        }

        protected override void OnInitialize()
        {
            m_Health = GetComponent<HealthComponent>();

            foreach (var property in Properties.Values)
            {
                property.Changed += OnPropertyChanged;
            }
        }

        protected override void OnTerminate()
        {
            foreach (var property in Properties.Values)
            {
                property.Changed -= OnPropertyChanged;
            }
        }

        private void OnPropertyChanged(Property property)
        {
            PropertyChanged?.Invoke(this, property);
        }

        public Property Get(PropertyType type)
        {
            return Properties[type];
        }

        public float AveragePhysicalResistance()
        {
            return Properties.Where(
                p => p.Key == PropertyType.PiercingResistance ||
                     p.Key == PropertyType.CrushingResistance ||
                     p.Key == PropertyType.PiercingResistance).Average(p => p.Value.Value());
        }

        public float AverageMagicalResistance()
        {
            return Properties.Where(
                p => p.Key == PropertyType.ArcaneResistance ||
                     p.Key == PropertyType.ColdResistance ||
                     p.Key == PropertyType.FireResistance ||
                     p.Key == PropertyType.HolyResistance ||
                     p.Key == PropertyType.LightningResistance ||
                     p.Key == PropertyType.PoisonResistance ||
                     p.Key == PropertyType.ShadowResistance).Average(p => p.Value.Value());
        }

        public Property Get(int id)
        {
            return Properties.Values.First(property => property.Id == id);
        }

        public void ApplyModifier(PropertyModifier modifier)
        {
            var fraction = m_Health.HealthFraction;

            modifier.Entity = gameObject;
            Get(modifier.Property.Type).AddModifier(modifier);

            m_Health.Health = m_Health.HealthMax * fraction;
        }

        public void RemoveModifier(PropertyModifier modifier)
        {
            var fraction = m_Health.HealthFraction;

            Get(modifier.Property.Type).RemoveModifier(modifier);

            m_Health.Health = m_Health.HealthMax * fraction;
        }

        public void ApplyModifiers(IEnumerable<PropertyModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                ApplyModifier(modifier);
            }
        }

        public void RemoveModifiers(IEnumerable<PropertyModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                RemoveModifier(modifier);
            }
        }

        public string GetAttackPowerString(GameObject entity)
        {
            return Get(PropertyType.AttackPower).ValueString();
        }

        public string GetSpellPowerString(GameObject entity)
        {
            return Get(PropertyType.SpellPower).ValueString();
        }

        public string GetCriticalChanceString(GameObject entity)
        {
            return Get(PropertyType.CriticalHitChance).ValueString();
        }

        public string GetCriticalDamageString(GameObject entity)
        {
            return Get(PropertyType.CriticalHitDamage).ValueString();
        }
    }
}