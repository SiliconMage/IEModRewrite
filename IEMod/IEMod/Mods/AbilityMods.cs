using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;
using static IEMod.Mods.AbilityMods;
using static UnityModManagerNet.UnityModManager.Param;

namespace IEMod.Mods
{
    internal class AbilityMods
    {
        public class AbilityMod
        {
            public string Name { get; set; }
            public int? MinimumDamage { get; set; }
            public int? MaximumDamage { get; set; }
            public int? AccuracyBonus { get; set; }
            public int? Range { get; set; }
            public int? DTBypass { get; set; }
            public int? Speed { get; set; }
            public int? Push { get; set; }
            public string DefendedBy { get; set; }
            public int? BlastRadius { get; set; }
            public int? BlastAngle { get; set; }
            public bool? Modal { get; set; }
            public string UseType { get; set; }
            public int? Uses { get; set; }
            public StatusEffectMod[] StatusEffectMods { get; set; }
            public bool? MainCharacterOnly { get; set; }
            public bool? PartyMembersOnly { get; set; }
        }

        public class StatusEffectMod
        {
            public float? Value { get; set; }
            public float? Duration { get; set; }
        }

        public class AbilityModList
        {
            public AbilityMod[] moddedAbilities { get; set; }
        }

        public static AbilityModList abilityMods { get; set; }

        /*
        [HarmonyPatch]
        static class GameResources_LoadPrefab_Patch
        {


            private static MethodBase TargetMethod() => AccessTools.Method(typeof(GameResources),"LoadPrefab", new Type[] { typeof(string), typeof(string), typeof(bool) },new Type[] {typeof(UnityEngine.Object)});

            static void Postfix(ref UnityEngine.Object __result, string filename, string assetName, bool instantiate)
            {
                if (__result != null)
                {
                    var gameObj = __result as GameObject;
                    var ability = gameObj?.GetComponent<GenericAbility>();

                    if (ModMain.Settings.LoadAbilityMods)
                    {
                        DirectoryInfo appDataDir = new DirectoryInfo(Application.dataPath);
                        DirectoryInfo modsDir = new DirectoryInfo(Path.Combine(appDataDir.Parent.FullName,
                            $"Mods{Path.DirectorySeparatorChar}IEMod"));

                        if (modsDir.Exists)
                        {

                            FileInfo abilityModsFile =
                                new FileInfo($"{modsDir.FullName}{Path.DirectorySeparatorChar}abilityMods.json");


                            if (abilityModsFile.Exists)
                            {
                                if (abilityMods == null)
                                {
                                    try
                                    {
                                        UnityModManager.Logger.Log("Initializing Ability Mods...");
                                        JsonSerializerSettings settings = new JsonSerializerSettings();
                                        settings.NullValueHandling = NullValueHandling.Ignore;
                                        abilityMods =
                                            JsonConvert.DeserializeObject<AbilityModList>(
                                                File.ReadAllText(abilityModsFile.FullName), settings);
                                        UnityModManager.Logger.Log("Initialization complete!");
                                    }
                                    catch (Exception e)
                                    {
                                        UnityModManager.Logger.Log(
                                            "An error occurred while trying to read abilityMods.json.  Please check the formatting of your file.");
                                        UnityModManager.Logger.Log($"Exception: {e.Message}");

                                        abilityMods = null;
                                    }
                                }

                                if (ability?.Owner != null)
                                {
                                    UnityModManager.Logger.Log($"Owner here?  {ability.Owner.gameObject}");
                                }
                                

                                if (abilityMods != null && ability != null)
                                {
                                    foreach (var abilityMod in abilityMods.moddedAbilities)
                                    {
                                        if (abilityMod.Name.Equals(ability.DisplayName?.GetText(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            UnityModManager.Logger.Log($"Modifying {abilityMod.Name}...");

                                            ApplyAbilityMod(ability, abilityMod);

                                            UnityModManager.Logger.Log("Modifications complete!");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                UnityModManager.Logger.Log("File abilityMods.json not found; skipping ability mods.");
                            }
                        }
                    }

                    if (ModMain.Settings.AllowCombatOnlyAbilitiesToBeUsedAnytime && ability != null)
                    {
                        ability.CombatOnly = false;
                    }
                }
            }
        }
        */

        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("Restored")]
        static class CharacterStats_Restored_Patch
        {
            static bool Prefix(CharacterStats __instance)
            {
                var self = Traverse.Create(__instance);

                if (ModMain.Settings.LoadAbilityMods)
                {
                    DirectoryInfo appDataDir = new DirectoryInfo(Application.dataPath);
                    DirectoryInfo modsDir = new DirectoryInfo(Path.Combine(appDataDir.Parent.FullName,
                        $"Mods{Path.DirectorySeparatorChar}IEMod"));

                    if (modsDir.Exists)
                    {

                        FileInfo abilityModsFile =
                            new FileInfo($"{modsDir.FullName}{Path.DirectorySeparatorChar}abilityMods.json");


                        if (abilityModsFile.Exists)
                        {
                            if (abilityMods == null)
                            {
                                try
                                {
                                    UnityModManager.Logger.Log("Initializing Ability Mods...");
                                    JsonSerializerSettings settings = new JsonSerializerSettings();
                                    settings.NullValueHandling = NullValueHandling.Ignore;
                                    abilityMods =
                                        JsonConvert.DeserializeObject<AbilityModList>(
                                            File.ReadAllText(abilityModsFile.FullName), settings);
                                    UnityModManager.Logger.Log("Initialization complete!");
                                }
                                catch (Exception e)
                                {
                                    UnityModManager.Logger.Log(
                                        "An error occurred while trying to read abilityMods.json.  Please check the formatting of your file.");
                                    UnityModManager.Logger.Log($"Exception: {e.Message}");

                                    abilityMods = null;
                                }
                            }
                        }

                        if (abilityMods != null)
                        {

                            foreach (var abilityMod in abilityMods.moddedAbilities)
                            {
                                bool mainCharacterOnly = false;
                                bool partyMemberOnly = false;

                                if (abilityMod.MainCharacterOnly.HasValue)
                                {
                                    mainCharacterOnly = abilityMod.MainCharacterOnly.Value;
                                }

                                if (abilityMod.PartyMembersOnly.HasValue)
                                {
                                    partyMemberOnly = abilityMod.PartyMembersOnly.Value;
                                }


                                if (mainCharacterOnly && __instance.GetComponent<Player>() == null)
                                {
                                    continue;
                                }

                                if (partyMemberOnly && !__instance.IsPartyMember)
                                {
                                    continue;
                                }


                                foreach (var talent in self.Field<BindingList<GenericTalent>>("m_talents").Value)
                                {

                                    if (abilityMod.Name.Equals(talent.DisplayName.GetText(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        UnityModManager.Logger.Log($"Modifying {abilityMod.Name}...");
                                        ApplyTalentMod(talent, abilityMod);
                                        UnityModManager.Logger.Log("Modifications complete!");
                                    }
                                }

                                foreach (var ability in self.Field<BindingList<GenericAbility>>("m_abilities").Value)
                                {
                                    if (abilityMod.Name.Equals(ability.DisplayName.GetText(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        UnityModManager.Logger.Log($"Modifying {abilityMod.Name}...");
                                        ApplyAbilityMod(ability, abilityMod);
                                        UnityModManager.Logger.Log("Modifications complete!");
                                    }
                                }
                            }
                        }
                    }
                }

                if (ModMain.Settings.AllowCombatOnlyAbilitiesToBeUsedAnytime)
                {
                    foreach (var ability in self.Field<BindingList<GenericAbility>>("m_abilities").Value)
                    {
                        ability.CombatOnly = false;
                    }
                }

                return true;
            }
        }

        private static void ApplyAbilityMod(GenericAbility ability, AbilityMod abilityMod)
        {
            AttackBase attackBase = ability.GetComponent<AttackBase>();
            AttackAOE attackAoE = ability.GetComponent<AttackAOE>();

            if (abilityMod.MinimumDamage.HasValue && attackBase != null)
            {
                attackBase.DamageData.Minimum = abilityMod.MinimumDamage.Value;
            }

            if (abilityMod.MaximumDamage.HasValue && attackBase != null)
            {
                attackBase.DamageData.Maximum = abilityMod.MaximumDamage.Value;
            }

            if (abilityMod.AccuracyBonus.HasValue && attackBase != null)
            {
                attackBase.AccuracyBonus = abilityMod.AccuracyBonus.Value;
            }

            if (abilityMod.Range.HasValue && attackBase != null)
            {
                attackBase.AttackDistance = abilityMod.Range.Value;
            }

            if (abilityMod.DTBypass.HasValue && attackBase != null)
            {
                attackBase.DTBypass = abilityMod.DTBypass.Value;
            }

            if (abilityMod.Speed.HasValue && attackBase != null)
            {
                AttackBase.AttackSpeedType newSpeedType = AttackBase.AttackSpeedType.Instant;

                switch (abilityMod.Speed.Value)
                {
                    case 1:
                        newSpeedType = AttackBase.AttackSpeedType.Short;
                        break;
                    case 2:
                        newSpeedType = AttackBase.AttackSpeedType.Long;
                        break;
                }

                attackBase.AttackSpeed = newSpeedType;
            }

            if (abilityMod.Push.HasValue && attackBase != null)
            {
                attackBase.PushDistance = abilityMod.Push.Value;
            }

            if (!string.IsNullOrWhiteSpace(abilityMod.DefendedBy) && attackBase != null)
            {
                CharacterStats.DefenseType newDefenseType = CharacterStats.DefenseType.None;

                switch (abilityMod.DefendedBy.ToUpper())
                {
                    case "DEFLECT":
                        newDefenseType = CharacterStats.DefenseType.Deflect;
                        break;
                    case "FORTITUDE":
                        newDefenseType = CharacterStats.DefenseType.Fortitude;
                        break;
                    case "REFLEX":
                        newDefenseType = CharacterStats.DefenseType.Reflex;
                        break;
                    case "WILL":
                        newDefenseType = CharacterStats.DefenseType.Will;
                        break;
                }

                attackBase.DefendedBy = newDefenseType;
            }

            if (abilityMod.BlastRadius.HasValue && attackAoE != null)
            {
                attackAoE.BlastRadius = abilityMod.BlastRadius.Value;
            }

            if (abilityMod.BlastAngle.HasValue && attackAoE != null)
            {
                attackAoE.DamageAngleDegrees = abilityMod.BlastAngle.Value;
            }

            if (abilityMod.StatusEffectMods != null)
            {
                for (int i = 0; i < abilityMod.StatusEffectMods.Length; i++)
                {
                    if (i < ability.StatusEffects.Length)
                    {
                        if (abilityMod.StatusEffectMods[i].Value.HasValue)
                        {
                            ability.StatusEffects[i].Value = abilityMod.StatusEffectMods[i].Value.Value;
                        }

                        if (abilityMod.StatusEffectMods[i].Duration.HasValue)
                        {
                            ability.StatusEffects[i].Duration = abilityMod.StatusEffectMods[i].Duration.Value;
                        }
                    }

                    if (attackAoE != null && i < attackAoE.StatusEffects.Count)
                    {
                        if (abilityMod.StatusEffectMods[i].Value.HasValue)
                        {
                            attackAoE.StatusEffects[i].Value = abilityMod.StatusEffectMods[i].Value.Value;
                        }

                        if (abilityMod.StatusEffectMods[i].Duration.HasValue)
                        {
                            attackAoE.StatusEffects[i].Duration = abilityMod.StatusEffectMods[i].Duration.Value;
                        }
                    }
                }
            }

            if (abilityMod.Modal.HasValue)
            {
                ability.Modal = abilityMod.Modal.Value;

                if (ability.Modal)
                {
                    ability.CooldownType = GenericAbility.CooldownMode.None;
                    ability.Cooldown = 0;

                    foreach (var statusEffect in ability.StatusEffects)
                    {
                        statusEffect.Duration = 0;
                    }
                }


            }

            if (!string.IsNullOrWhiteSpace(abilityMod.UseType))
            {
                GenericAbility.CooldownMode newCooldown = GenericAbility.CooldownMode.None;

                switch (abilityMod.UseType.ToUpper())
                {
                    case "PERENCOUNTER":
                        newCooldown = GenericAbility.CooldownMode.PerEncounter;
                        break;
                    case "PERREST":
                        newCooldown = GenericAbility.CooldownMode.PerRest;
                        break;
                    case "CHARGED":
                        newCooldown = GenericAbility.CooldownMode.Charged;
                        break;
                    case "PERSTRONGHOLDTURN":
                        newCooldown = GenericAbility.CooldownMode.PerStrongholdTurn;
                        break;
                }

                ability.CooldownType = newCooldown;
            }

            if (abilityMod.Uses.HasValue)
            {
                ability.Cooldown = abilityMod.Uses.Value;
            }

        }

        private static void ApplyTalentMod(GenericTalent talent, AbilityMod abilityMod)
        {

            if (abilityMod.StatusEffectMods != null)
            {
                if (talent.Type == GenericTalent.TalentType.ModExistingAbility)
                {

                    foreach (var mod in talent.AbilityMods)
                    {
                        for (int i = 0; i < abilityMod.StatusEffectMods.Length; i++)
                        {
                            if (i < mod.StatusEffects.Length)
                            {
                                if (abilityMod.StatusEffectMods[i].Value.HasValue)
                                {
                                    mod.StatusEffects[i].Value = abilityMod.StatusEffectMods[i].Value.Value;
                                }

                                if (abilityMod.StatusEffectMods[i].Duration.HasValue)
                                {
                                    mod.StatusEffects[i].Duration = abilityMod.StatusEffectMods[i].Duration.Value;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var ability in talent.Abilities)
                    {
                        for (int i = 0; i < abilityMod.StatusEffectMods.Length; i++)
                        {
                            if (i < ability.StatusEffects.Length)
                            {
                                if (abilityMod.StatusEffectMods[i].Value.HasValue)
                                {
                                    ability.StatusEffects[i].Value = abilityMod.StatusEffectMods[i].Value.Value;
                                }

                                if (abilityMod.StatusEffectMods[i].Duration.HasValue)
                                {
                                    ability.StatusEffects[i].Duration = abilityMod.StatusEffectMods[i].Duration.Value;
                                }
                            }
                        }
                    }
                }
            }

            if (abilityMod.Modal.HasValue && abilityMod.Modal.Value)
            {
                if (talent.Type == GenericTalent.TalentType.ModExistingAbility)
                {
                    foreach (var mod in talent.AbilityMods)
                    {
                        foreach (var statEffect in mod.StatusEffects)
                        {
                            statEffect.Duration = 0;
                        }
                    }
                }
                else
                {
                    foreach (var ability in talent.Abilities)
                    {
                        foreach (var statEffect in ability.StatusEffects)
                        {
                            statEffect.Duration = 0;
                        }
                    }
                }
            }
        }
    }
}
