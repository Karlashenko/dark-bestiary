﻿using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Data;
using DarkBestiary.Extensions;
using DarkBestiary.GameBoard;
using DarkBestiary.Validators;
using DarkBestiary.Values;
using UnityEngine;

namespace DarkBestiary.Behaviours
{
    public class PerSurroundingEnemyDamageBehaviour : DamageBehaviour
    {
        private readonly PerSurroundingEnemyDamageBehaviourData m_Data;

        public PerSurroundingEnemyDamageBehaviour(PerSurroundingEnemyDamageBehaviourData data,
            List<ValidatorWithPurpose> validators) : base(data, validators)
        {
            m_Data = data;
        }

        protected override float OnGetDamageMultiplier(GameObject victim, GameObject attacker, ref Damage damage)
        {
            var enemyCount = BoardNavigator.Instance
                .EntitiesInRadius(attacker.transform.position, m_Data.Range)
                .Count(entity => entity.IsEnemyOf(attacker));

            if (enemyCount < m_Data.MinimumNumberOfEnemies)
            {
                return 0;
            }

            return Mathf.Clamp(
                enemyCount * (m_Data.AmountPerEnemy * StackCount),
                m_Data.Min,
                m_Data.Max
            );
        }
    }
}