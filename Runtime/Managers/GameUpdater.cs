﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace DarkBestiary.Managers
{
    public class GameUpdater : Singleton<GameUpdater>
    {
        public event Action UpdateStarted;
        public event Action UpdateCompleted;
        public event Action<string> UpdateProgress;

        [SerializeField] private string m_Host;

        public void StartUpdate()
        {
            StartCoroutine(Download());
        }

        private IEnumerator Download()
        {
            UpdateStarted?.Invoke();

            var links = new Dictionary<string, string>
            {
                {"units", "/api/units"},
                {"unit_groups", "/api/unit_groups"},
                {"archetypes", "/api/archetypes"},
                {"rewards", "/api/rewards"},
                {"loot", "/api/loot"},
                {"items", "/api/items"},
                {"item_sets", "/api/item_sets"},
                {"item_types", "/api/item_types"},
                {"item_categories", "/api/item_categories"},
                {"item_rarities", "/api/item_rarities"},
                {"behaviours", "/api/behaviours"},
                {"skills", "/api/skills"},
                {"skins", "/api/skins"},
                {"missiles", "/api/missiles"},
                {"effects", "/api/effects"},
                {"scenes", "/api/scenes"},
                {"scenarios", "/api/scenarios"},
                {"properties", "/api/properties"},
                {"attributes", "/api/attributes"},
                {"environments", "/api/environments"},
                {"currencies", "/api/currencies"},
                {"ai", "/api/ai"},
                {"talents", "/api/talents"},
                {"relics", "/api/relics"},
                {"talent_categories", "/api/talent_categories"},
                {"recipes", "/api/recipes"},
                {"validators", "/api/validators"},
                {"achievements", "/api/achievements"},
                {"backgrounds", "/api/backgrounds"},
                {"item_modifiers", "/api/item_modifiers"},
                {"skill_categories", "/api/skill_categories"},
                {"skill_sets", "/api/skill_sets"},
                {"masteries", "/api/masteries"},
                {"map_encounters", "/api/map_encounters"},
                {"food", "/api/food"},
                {"i18n", "/api/i18n"},
            };

            yield return StartCoroutine(Download(links, "core/data"));

            links = new Dictionary<string, string>
            {
                {"ru-RU", "/api/i18n/ru"},
                {"en-US", "/api/i18n/en"},
            };

            yield return StartCoroutine(Download(links, "core/i18n"));

            UpdateCompleted?.Invoke();
        }

        private IEnumerator Download(Dictionary<string, string> links, string directory)
        {
            foreach (var link in links)
            {
                UpdateProgress?.Invoke(link.Key);

                using (var request = UnityWebRequest.Get(m_Host + link.Value))
                {
                    yield return request.SendWebRequest();

                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.LogError(request.url + " " + request.error);
                        break;
                    }

                    File.WriteAllText(
                        Environment.s_StreamingAssetsPath + "/" + directory + "/" + link.Key + ".json",
                        request.downloadHandler.text);
                }
            }
        }
    }
}