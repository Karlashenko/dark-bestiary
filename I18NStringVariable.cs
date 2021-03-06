﻿using System;
using System.Collections.Generic;
using System.Reflection;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Data.Repositories;
using DarkBestiary.Managers;
using DarkBestiary.Skills;
using UnityEngine;

namespace DarkBestiary
{
    public struct StringVariableContext
    {
        public GameObject Entity { get; }
        public Skill Skill { get; }
        public int StackCount { get; }

        public StringVariableContext(GameObject entity) : this()
        {
            Entity = entity;
            Skill = null;
            StackCount = 0;
        }

        public StringVariableContext(GameObject entity, Skill skill)
        {
            Entity = entity;
            Skill = skill;
            StackCount = 0;
        }

        public StringVariableContext(GameObject entity, int stackCount)
        {
            Entity = entity;
            Skill = null;
            StackCount = stackCount;
        }

        public StringVariableContext(GameObject entity, Skill skill, int stackCount)
        {
            Entity = entity;
            Skill = skill;
            StackCount = stackCount;
        }
    }

    public class I18NStringVariable
    {
        private static readonly Dictionary<string, Delegate> Cache =
            new Dictionary<string, Delegate>();

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.IgnoreCase |
                                                  System.Reflection.BindingFlags.Public;

        public I18NStringVariableData Data { get; }

        public I18NStringVariable(I18NStringVariableData data)
        {
            Data = data;
        }

        public object GetValue(StringVariableContext context)
        {
            return CreateMethodDelegate(context).Method.Invoke(GetSubject(context), new object[] {context.Entity});
        }

        private Delegate CreateMethodDelegate(StringVariableContext context)
        {
            var key = GetCacheKey();

            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }

            var subject = GetSubject(context);
            var method = subject.GetType().GetMethod(Data.PropertyName, BindingFlags);

            if (method == null)
            {
                throw new Exception($"{subject.GetType()} doesn't have method with name {Data.PropertyName}");
            }

            return Cache[key] = Delegate.CreateDelegate(typeof(Func<GameObject, string>), subject, method);
        }

        private object GetSubject(StringVariableContext context)
        {
            switch (Data.EntityType)
            {
                case "Effect":
                    var effect = Container.Instance.Resolve<IEffectRepository>().FindOrFail(Data.EntityId);
                    effect.StackCount = context.StackCount;
                    effect.Skill = context.Skill;
                    return effect;
                case "Behaviour":
                    var behaviour = Container.Instance.Resolve<IBehaviourRepository>().FindOrFail(Data.EntityId);
                    behaviour.StackCount = context.StackCount;
                    return behaviour;
                case "Mastery":
                    var mastery = CharacterManager.Instance.Character.Entity.GetComponent<MasteriesComponent>().Find(Data.EntityId);
                    return mastery;
                case "Property":
                    return CharacterManager.Instance.Character.Entity.GetComponent<PropertiesComponent>();
                default:
                    throw new Exception($"Invalid entity type {Data.EntityType}");
            }
        }

        private string GetCacheKey()
        {
            return Data.EntityType + Data.EntityId + Data.PropertyName;
        }
    }
}