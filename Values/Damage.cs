﻿using UnityEngine;

namespace DarkBestiary.Values
{
    public struct Damage
    {
        public float Amount { get; set; }
        public float Absorbed { get; set; }
        public DamageType Type { get; set; }
        public DamageFlags Flags { get; set; }
        public DamageInfoFlags InfoFlags { get; set; }
        public WeaponSound WeaponSound { get; set; }

        public Damage(float amount, DamageType type)
        {
            Amount = amount;
            Type = type;
            WeaponSound = WeaponSound.None;
            Flags = DamageFlags.None;
            InfoFlags = DamageInfoFlags.None;
            Absorbed = 0;
        }

        public Damage(float amount, DamageType type, DamageFlags flags, DamageInfoFlags infoFlags)
        {
            Amount = amount;
            Type = type;
            Flags = flags;
            InfoFlags = infoFlags;
            WeaponSound = WeaponSound.None;
            Absorbed = 0;
        }

        public Damage(float amount, DamageType type, WeaponSound weaponSound, DamageFlags flags, DamageInfoFlags infoFlags)
        {
            Amount = amount;
            Type = type;
            WeaponSound = weaponSound;
            Flags = flags;
            InfoFlags = infoFlags;
            Absorbed = 0;
        }

        public bool IsWeapon()
        {
            return Flags.HasFlag(DamageFlags.Melee) || Flags.HasFlag(DamageFlags.Ranged);
        }

        public bool IsMagic()
        {
            return Flags.HasFlag(DamageFlags.Magic);
        }

        public bool IsPhysicalType()
        {
            return Type == DamageType.Crushing ||
                   Type == DamageType.Slashing ||
                   Type == DamageType.Piercing;
        }

        public bool IsMagicalType()
        {
            return Type == DamageType.Fire ||
                   Type == DamageType.Cold ||
                   Type == DamageType.Holy ||
                   Type == DamageType.Shadow ||
                   Type == DamageType.Arcane ||
                   Type == DamageType.Lightning ||
                   Type == DamageType.Poison;
        }

        public bool IsDOT()
        {
            return Flags.HasFlag(DamageFlags.DOT);
        }

        public bool IsReflected()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.Reflected);
        }

        public bool IsCritical()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.Critical);
        }

        public bool IsSpiritLink()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.SpiritLink);
        }

        public bool IsInvulnerable()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.Invulnerable);
        }

        public bool IsDodged()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.Dodged);
        }

        public bool IsBlocked()
        {
            return InfoFlags.HasFlag(DamageInfoFlags.Blocked);
        }

        public bool IsBlockedOrDodged()
        {
            return IsBlocked() || IsDodged();
        }

        public static Damage operator +(Damage damage, float amount)
        {
            return new Damage(damage.Amount + amount, damage.Type, damage.WeaponSound, damage.Flags, damage.InfoFlags);
        }

        public static Damage operator -(Damage damage, float amount)
        {
            return new Damage(
                Mathf.Max(0, damage.Amount - amount), damage.Type, damage.WeaponSound, damage.Flags, damage.InfoFlags);
        }

        public static Damage operator *(Damage damage, float amount)
        {
            return new Damage(damage.Amount * amount, damage.Type, damage.WeaponSound, damage.Flags, damage.InfoFlags);
        }

        public static Damage operator /(Damage damage, float amount)
        {
            return new Damage(damage.Amount / amount, damage.Type, damage.WeaponSound, damage.Flags, damage.InfoFlags);
        }

        public static implicit operator float(Damage damage)
        {
            return damage.Amount;
        }
    }
}