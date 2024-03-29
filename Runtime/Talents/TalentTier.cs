using System;
using System.Collections.Generic;

namespace DarkBestiary.Talents
{
    public class TalentTier
    {
        public event Action<TalentTier> Locked;
        public event Action<TalentTier> Unlocked;

        public TalentCategory Category { get; }
        public int Index { get; }
        public List<Talent> Talents { get; }
        public bool IsUnlocked { get; private set; }

        public TalentTier(TalentCategory category, int index, List<Talent> talents, bool isUnlocked)
        {
            Category = category;
            Index = index;
            Talents = talents;
            IsUnlocked = isUnlocked;
        }

        public void Lock()
        {
            IsUnlocked = false;
            Locked?.Invoke(this);
        }

        public void Unlock()
        {
            IsUnlocked = true;
            Unlocked?.Invoke(this);
        }
    }
}