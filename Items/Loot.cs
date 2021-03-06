﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DarkBestiary.Data;
using DarkBestiary.Data.Repositories;
using DarkBestiary.Extensions;
using DarkBestiary.Randomization;

namespace DarkBestiary.Items
{
    public class Loot
    {
        private readonly IItemRepository itemRepository;
        private readonly ILootDataRepository lootDataRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IRandomizerTable main;

        private List<Item> result;

        public Loot(LootData data, IItemRepository itemRepository,
            ILootDataRepository lootDataRepository, IItemCategoryRepository itemCategoryRepository)
        {
            this.itemRepository = itemRepository;
            this.lootDataRepository = lootDataRepository;
            this.itemCategoryRepository = itemCategoryRepository;

            this.main = CreateTable(data, 0, true, true, true);
        }

        public void RollDropAsync(int level, Action<List<Item>> callback)
        {
            this.result = null;

            new Thread(() => this.result = RollDrop(level)).Start();

            Timer.Instance.WaitUntil(() => this.result != null, () => callback.Invoke(this.result));
        }

        public List<Item> RollDrop(int level)
        {
            AssignMonsterLevel(this.main, level);

            return this.main.Evaluate()
                .Select(random => ((RandomizerItemValue) random)?.Value?.Clone())
                .NotNull()
                .ToList();
        }

        private static void AssignMonsterLevel(IRandomizerObject randomizerObject, int level)
        {
            if (randomizerObject is IRandomizerTable table)
            {
                foreach (var value in table.Contents)
                {
                    AssignMonsterLevel(value, level);
                }

                return;
            }

            if (randomizerObject is RandomizerRandomItemValue randomItemValue)
            {
                randomItemValue.MonsterLevel = level;
            }
        }

        private IRandomizerTable CreateTable(
            LootData data, float probability, bool unique, bool guaranteed, bool enabled)
        {
            var table = new RandomizerTable(data.Count, probability, unique, guaranteed, enabled);

            foreach (var item in data.Items)
            {
                switch (item.Type)
                {
                    case LootItemType.Null:
                        table.Contents.Add(new RandomizerNullValue(item.Probability));
                        break;
                    case LootItemType.Table:
                        table.Contents.Add(
                            CreateTable(
                                this.lootDataRepository.FindOrFail(item.TableId),
                                item.Probability,
                                item.Unique,
                                item.Guaranteed,
                                item.Enabled
                            )
                        );
                        break;
                    case LootItemType.Item:
                        table.Contents.Add(CreateItem(item));
                        break;
                    case LootItemType.Random:
                        table.Contents.Add(CreateRandom(item));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(item.Type), item.Type, null);
                }
            }

            return table;
        }

        private IRandomizerObject CreateRandom(LootItemData data)
        {
            return new RandomizerRandomItemValue(data, this.itemRepository, this.itemCategoryRepository);
        }

        private IRandomizerObject CreateItem(LootItemData data)
        {
            return new RandomizerItemValue(this.itemRepository.FindOrFail(data.ItemId), data);
        }
    }
}