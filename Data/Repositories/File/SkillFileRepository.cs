﻿using System;
using System.Collections.Generic;
using System.Linq;
using DarkBestiary.Data.Mappers;
using DarkBestiary.Data.Readers;
using DarkBestiary.Extensions;
using DarkBestiary.Skills;

namespace DarkBestiary.Data.Repositories.File
{
    public class SkillFileRepository : FileRepository<int, SkillData, Skill>, ISkillRepository
    {
        public SkillFileRepository(IFileReader reader, SkillMapper mapper) : base(reader, mapper)
        {
        }

        protected override string GetFilename()
        {
            return Environment.StreamingAssetsPath + "/compiled/data/skills.json";
        }

        public List<Skill> Find(Func<SkillData, bool> predicate)
        {
            return LoadData()
                .Where(predicate)
                .Select(this.Mapper.ToEntity)
                .ToList();
        }

        public List<Skill> Tradable(Func<SkillData, bool> predicate = null)
        {
            if (predicate == null)
            {
                predicate = _ => true;
            }

            return LoadData()
                .Tradable(predicate)
                .Select(this.Mapper.ToEntity)
                .ToList();
        }
    }
}