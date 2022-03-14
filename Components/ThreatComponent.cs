using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Extensions;
using DarkBestiary.Messaging;
using DarkBestiary.Scenarios.Encounters;
using DarkBestiary.Scenarios.Scenes;
using UnityEngine;

namespace DarkBestiary.Components
{
    public class ThreatComponent : Component
    {
        private const float InitialThreatAmount = 10;
        private const float RangeMultiplierDividend = 25;
        private const float HealingThreatMultiplier = 0.25f;

        public Dictionary<GameObject, float> Table { get; } = new Dictionary<GameObject, float>();

        public GameObject Top()
        {
            return Table.Where(entry => !entry.Key.IsInvisible())
                .OrderByDescending(temp => temp.Value)
                .FirstOrDefault().Key;
        }

        public float ThreatFrom(GameObject entity)
        {
            if (!Table.TryGetValue(entity, out var threat))
            {
                return 0;
            }

            return threat;
        }

        protected override void OnInitialize()
        {
            UnitComponent.AnyUnitInitialized += OnAnyUnitInitialized;
            UnitComponent.AnyUnitTerminated += OnAnyUnitTerminated;
            UnitComponent.AnyUnitOwnerChanged += OnAnyUnitOwnerChanged;
            CombatEncounter.AnyCombatStarted += OnAnyCombatStarted;
            HealthComponent.AnyEntityHealed += OnAnyEntityHealed;
            HealthComponent.AnyEntityDied += OnAnyEntityDied;
            GetComponent<HealthComponent>().Damaged += OnDamaged;

            Timer.Instance.WaitForFixedUpdate(FindEnemies);
        }

        protected override void OnTerminate()
        {
            UnitComponent.AnyUnitInitialized -= OnAnyUnitInitialized;
            UnitComponent.AnyUnitTerminated -= OnAnyUnitTerminated;
            UnitComponent.AnyUnitOwnerChanged -= OnAnyUnitOwnerChanged;
            CombatEncounter.AnyCombatStarted -= OnAnyCombatStarted;
            HealthComponent.AnyEntityHealed -= OnAnyEntityHealed;
            HealthComponent.AnyEntityDied -= OnAnyEntityDied;
            GetComponent<HealthComponent>().Damaged -= OnDamaged;
        }

        private void FindEnemies()
        {
            if (Scene.Active == null)
            {
                return;
            }

            foreach (var entity in Scene.Active.Entities.EnemiesOf(gameObject))
            {
                AddRawThreat(entity.gameObject, InitialThreatAmount);
            }
        }

        private void OnAnyCombatStarted(CombatEncounter combat)
        {
            Table.Clear();
            FindEnemies();
        }

        private void OnAnyUnitOwnerChanged(UnitComponent unit)
        {
            if (!Table.ContainsKey(unit.gameObject) || unit.gameObject.IsEnemyOf(gameObject))
            {
                return;
            }

            Table.Remove(unit.gameObject);
        }

        private void OnAnyUnitInitialized(UnitComponent unit)
        {
            AddRawThreat(unit.gameObject, InitialThreatAmount);
        }

        private void OnAnyUnitTerminated(UnitComponent unit)
        {
            Table.RemoveIfExists(unit.gameObject);
        }

        private void OnAnyEntityHealed(EntityHealedEventData data)
        {
            if (!data.Healer.IsEnemyOf(gameObject))
            {
                return;
            }

            AddRawThreat(data.Healer, data.Healing.Amount * HealingThreatMultiplier);
        }

        private void OnAnyEntityDied(EntityDiedEventData payload)
        {
            Table.RemoveIfExists(payload.Victim);
        }

        private void OnDamaged(EntityDamagedEventData data)
        {
            AddRawThreat(data.Attacker, data.Damage.Amount);
        }

        private void AddRawThreat(GameObject entity, float threat)
        {
            if (entity.IsDummy())
            {
                return;
            }

            var distanceMultiplier = RangeMultiplierDividend / Mathf.Max(1, (entity.transform.position - transform.position).sqrMagnitude);

            Table.AddIfNotExists(entity, 0);
            Table[entity] += threat * distanceMultiplier;
        }
    }
}