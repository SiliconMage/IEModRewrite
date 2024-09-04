using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class CompanionStatsMod
    {
        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("Restored")]
        static class CharacterStats_Restored_Patch
        {
            public static CompanionStatsModList companionStatsMods;

            static void Postfix(CharacterStats __instance)
            {
                if (ModMain.Settings.EnableCompanionStatModifications)
                {
                    if (companionStatsMods == null)
                    {
                        companionStatsMods = InitializeCompanionStatsModifiers();
                    }

                    CompanionStatsModifiers statMods = null;

                    bool isPlayerCharacter = __instance.GetComponent<Player>() != null;
                    bool isHiredAdventure = !isPlayerCharacter && __instance.GetComponent<CompanionInstanceID>() == null;

                    if (isPlayerCharacter)
                    {
                        statMods = companionStatsMods.companionStats.FirstOrDefault(x => x.Name == "Main Character");
                    }

                    if (isHiredAdventure)
                    {
                        statMods = companionStatsMods.companionStats.FirstOrDefault(x => x.Name == __instance.OverrideName);
                    }

                    if (!isPlayerCharacter && (isHiredAdventure || __instance.IsPartyMember) && statMods == null)
                    {
                        statMods = companionStatsMods.companionStats.FirstOrDefault(x => x.Name == __instance.DisplayName.ToString());
                    }

                    if (statMods != null)
                    {
                        UnityModManager.Logger.Log($"Applying modifications to {statMods.Name}...");

                        if (statMods.Might.HasValue)
                        {
                            __instance.BaseMight = statMods.Might.Value;
                        }

                        if (statMods.Constitution.HasValue)
                        {
                            __instance.BaseConstitution = statMods.Constitution.Value;
                        }

                        if (statMods.Dexterity.HasValue)
                        {
                            __instance.BaseDexterity = statMods.Dexterity.Value;
                        }

                        if (statMods.Perception.HasValue)
                        {
                            __instance.BasePerception = statMods.Perception.Value;
                        }

                        if (statMods.Intellect.HasValue)
                        {
                            __instance.BaseIntellect = statMods.Intellect.Value;
                        }

                        if (statMods.Resolve.HasValue)
                        {
                            __instance.BaseResolve = statMods.Resolve.Value;
                        }

                        if (statMods.Athletics.HasValue)
                        {
                            __instance.AthleticsBonus += statMods.Athletics.Value;
                        }

                        if (statMods.Lore.HasValue)
                        {
                            __instance.LoreBonus += statMods.Lore.Value;
                        }

                        if (statMods.Mechanics.HasValue)
                        {
                            __instance.MechanicsBonus += statMods.Mechanics.Value;
                        }

                        if (statMods.Stealth.HasValue)
                        {
                            __instance.StealthBonus += statMods.Stealth.Value;
                        }

                        if (statMods.Survival.HasValue)
                        {
                            __instance.SurvivalBonus += statMods.Survival.Value;
                        }

                        UnityModManager.Logger.Log($"Modifications for {statMods.Name} complete!");

                    }
                }
            }

            private static CompanionStatsModList InitializeCompanionStatsModifiers()
            {
                CompanionStatsModList retVal = new CompanionStatsModList();
                retVal.companionStats = Array.Empty<CompanionStatsModifiers>();

                DirectoryInfo appDataDir = new DirectoryInfo(Application.dataPath);
                DirectoryInfo modsDir = new DirectoryInfo(Path.Combine(appDataDir.Parent.FullName, $"Mods{Path.DirectorySeparatorChar}IEMod"));

                if (modsDir.Exists)
                {
                    FileInfo companionStatsModFile = new FileInfo($"{modsDir.FullName}{Path.DirectorySeparatorChar}companionStats.json");

                    if (companionStatsModFile.Exists)
                    {
                        UnityModManager.Logger.Log("Initializing Companion Stat Modifications...");
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.NullValueHandling = NullValueHandling.Ignore;

                        try
                        {
                            retVal = JsonConvert.DeserializeObject<CompanionStatsModList>(File.ReadAllText(companionStatsModFile.FullName), settings);
                        }
                        catch (Exception e)
                        {
                            UnityModManager.Logger.Log("An error occurred while attempting to read companionStats.json.  Please check the formatting of your file.");
                            UnityModManager.Logger.Log($"Exception: {e.Message}");
                            retVal = new CompanionStatsModList()
                                { companionStats = Array.Empty<CompanionStatsModifiers>() };
                        }
                        finally
                        {
                            UnityModManager.Logger.Log("Initialization complete.");
                        }
                    }
                }


                return retVal;
            }
        }

        public class CompanionStatsModList
        {
            public CompanionStatsModifiers[] companionStats { get; set; }
        }

        public class CompanionStatsModifiers
        {
            public string Name { get; set; }
            public int? Might { get; set; }
            public int? Constitution { get; set; }
            public int? Dexterity { get; set; }
            public int? Intellect { get; set; }
            public int? Perception { get; set; }
            public int? Resolve { get; set; }
            public int? Athletics { get; set; }
            public int? Lore { get; set; }
            public int? Mechanics { get; set; }
            public int? Stealth { get; set; }
            public int? Survival { get; set; }
        }
    }
}
