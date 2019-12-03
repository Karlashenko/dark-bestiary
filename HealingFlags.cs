﻿using System;
using Newtonsoft.Json;

namespace DarkBestiary
{
    [Flags]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum HealingFlags
    {
        None = 0,
        Vampirism = 1 << 1,
        Regeneration = 1 << 2,
    }
}