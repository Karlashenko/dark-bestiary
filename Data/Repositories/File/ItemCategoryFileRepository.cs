﻿using System.Linq;
using DarkBestiary.Data.Mappers;
using DarkBestiary.Data.Readers;
using DarkBestiary.Items;

namespace DarkBestiary.Data.Repositories.File
{
    public class ItemCategoryFileRepository : FileRepository<int, ItemCategoryData, ItemCategory>, IItemCategoryRepository
    {
        public ItemCategoryFileRepository(IFileReader reader, ItemCategoryMapper mapper) : base(reader, mapper)
        {
        }

        protected override string GetFilename()
        {
            return Environment.StreamingAssetsPath + "/compiled/data/item_categories.json";
        }

        public ItemCategory FindByType(ItemCategoryType type)
        {
            return LoadData().Where(data => data.Type == type).Select(this.Mapper.ToEntity).FirstOrDefault();
        }
    }
}