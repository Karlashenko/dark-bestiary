﻿using DarkBestiary.Components;
using DarkBestiary.Exceptions;

namespace DarkBestiary.Items
{
    public class NotEquippableEquipmentStrategy : IEquipmentStrategy
    {
        public void Prepare(Item item, EquipmentSlot slot, EquipmentComponent equipment)
        {
        }
    }
}