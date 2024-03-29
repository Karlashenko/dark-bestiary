﻿using System;
using System.Collections.Generic;
using DarkBestiary.Items;
using DarkBestiary.Items.Transmutation.Recipes;
using DarkBestiary.UI.Elements;

namespace DarkBestiary.UI.Views
{
    public interface ITransmutationView : IView, IHideOnEscape
    {
        event Action Combine;
        event Action<Item> AddItem;
        event Action<Item, int> AddItemIndex;
        event Action<Item> RemoveItem;
        event Action<Item, Item> SwapItems;

        void Construct(InventoryPanel inventoryPanel, List<Item> items, List<ITransmutationRecipe> recipes);
        void RefreshItems(List<Item> items);
        void ShowResult(Recipe recipe);
        void ClearResult();
        void ShowResult(string name, string description);
        void ShowImpossibleCombination();
        void OnSuccess();
        void Block(Item item);
        void Unblock(Item item);
    }
}