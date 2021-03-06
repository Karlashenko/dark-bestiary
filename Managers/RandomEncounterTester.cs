using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Data;
using DarkBestiary.Data.Repositories;
using DarkBestiary.Extensions;
using UnityEngine;

namespace DarkBestiary.Managers
{
    public class RandomEncounterTester : MonoBehaviour
    {
        [Space(20)]
        [SerializeField] private int challengeRating;
        [SerializeField] private int environmentId;

        public List<UnitData> Test()
        {
            var units = Container.Instance.Resolve<IUnitDataRepository>().FindAll()
                .Where(unit => this.environmentId == 0 || unit.Environment.Id == this.environmentId)
                .Where(unit => !unit.Flags.HasFlag(UnitFlags.Boss) &&
                               !unit.Flags.HasFlag(UnitFlags.Summoned) &&
                               !unit.Flags.HasFlag(UnitFlags.Dummy) &&
                               !unit.Flags.HasFlag(UnitFlags.Playable))
                .ToList();

            var result = new List<UnitData>();
            var iterations = 0;

            while (true)
            {
                var possible = units
                    .Where(u1 => u1.ChallengeRating <= this.challengeRating - result.Sum(u2 => u2.ChallengeRating))
                    .ToList();

                if (possible.Count == 0)
                {
                    break;
                }

                result.Add(possible.Random());

                if (++iterations > 1000)
                {
                    Debug.LogWarning("Maximum iterations hit!");
                    break;
                }
            }

            return result;
        }
    }
}