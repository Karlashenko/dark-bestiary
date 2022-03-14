using System;
using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Components;
using DarkBestiary.Data;
using DarkBestiary.Data.Mappers;
using DarkBestiary.Data.Readers;
using DarkBestiary.Items;
using DarkBestiary.Messaging;
using UnityEngine;
using Zenject;

namespace DarkBestiary.Managers
{
    public class Mailbox : IInitializable
    {
        public static Mailbox Instance { get; private set; }

        public event Payload Updated;

        public IReadOnlyCollection<Item> Items => this.items;

        private readonly ItemSaveDataMapper mapper;
        private readonly IFileReader reader;
        private readonly StorageId storageId;

        private List<Item> items;

        public Mailbox(ItemSaveDataMapper mapper, IFileReader reader, StorageId storageId)
        {
            this.mapper = mapper;
            this.reader = reader;
            this.storageId = storageId;
        }

        public void Initialize()
        {
            var data = new List<ItemSaveData>();

            try
            {
                data = this.reader.Read<List<ItemSaveData>>(GetDataPath()) ?? new List<ItemSaveData>();
            }
            catch (Exception exception)
            {
                // ignored
            }

            this.items = data.Where(d => d.ItemId > 0).Select(d => this.mapper.ToEntity(d)).ToList();

            Application.quitting += OnApplicationQuitting;

            Instance = this;
        }

        private void OnApplicationQuitting()
        {
            var data = this.items.Select(i => this.mapper.ToData(i)).ToList();

            this.reader.Write(data, GetDataPath());
        }

        public void SendMail(Item item)
        {
            this.items.Add(item);
            Updated?.Invoke();
        }

        public void SendMail(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                SendMail(item);
            }
        }

        public void RemoveMail(Item item)
        {
            this.items.Remove(item);
            Updated?.Invoke();
        }

        public void TakeMail(Item item, GameObject entity)
        {
            RemoveMail(item);

            entity.GetComponent<InventoryComponent>().Pickup(item);

            Updated?.Invoke();
        }

        public void TakeAll(GameObject entity)
        {
            var inventory = entity.GetComponent<InventoryComponent>();

            while (true)
            {
                if (this.items.Count == 0 || inventory.GetFreeSlotCount() == 0)
                {
                    break;
                }

                inventory.Pickup(this.items[0]);
                this.items.Remove(this.items[0]);
            }

            Updated?.Invoke();
        }

        private string GetDataPath()
        {
            return Environment.PersistentDataPath + $"/{this.storageId}/mailbox.save";
        }
    }
}