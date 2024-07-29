using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class WizardMods
    {
        [HarmonyPatch(typeof(UIGrimoireInSpellRow))]
        [HarmonyPatch("Init")]
        public static class UIGrimoireInSpellRow_Init_Patch
        { 
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
            {
                
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {
                    int totalSlots = 4 + ModMain.Settings.BonusWizardPreparationSlots;
                    int maxLoopValueIndex = -1;

                    var codes = new List<CodeInstruction>(codeInstructions);

                    for (int i = 0; i < codes.Count; i++)
                    {
                        if (codes[i].opcode == OpCodes.Ldc_I4_4)
                        {
                            maxLoopValueIndex = i;
                            break;
                        }
                    }

                    if (maxLoopValueIndex > -1)
                    {
                        OpCode newOpCode = OpCodes.Ldc_I4_4;

                        switch (totalSlots)
                        {
                            case 5:
                                newOpCode = OpCodes.Ldc_I4_5;
                                break;
                            case 6:
                                newOpCode = OpCodes.Ldc_I4_6;
                                break;
                            case 7:
                                newOpCode = OpCodes.Ldc_I4_7;
                                break;
                        }

                        codes[maxLoopValueIndex].opcode = newOpCode;
                    }

                    return codes.AsEnumerable();

                }

                return codeInstructions;
            }
        }

        [HarmonyPatch(typeof(UIGrimoireInSpellRow))]
        [HarmonyPatch("Reload")]
        static class UIGrimoireInSpellRow_Reload_Patch
        {
            static bool Prefix(UIGrimoireInSpellRow __instance, int spellLevel)
            {
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {
                    var self = Traverse.Create(__instance);

                    self.Field("m_SpellLevel").SetValue(spellLevel);
                    int i = 0;
                    Grimoire loadedGrimoire = UIGrimoireManager.Instance.LoadedGrimoire;
                    if (loadedGrimoire && spellLevel - 1 < loadedGrimoire.Spells.Length)
                    {
                        foreach (GenericSpell genericSpell in loadedGrimoire.Spells[spellLevel - 1].SpellData)
                        {
                            if (!(genericSpell == null))
                            {
                                if (i >= self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>().Count)
                                {
                                    Debug.LogWarning(string.Concat(new object[]
                                    {
                                        "Grimoire has too many spells for UI (",
                                        UIGrimoireManager.Instance.LoadedGrimoire.name,
                                        " S.L.",
                                        spellLevel,
                                        ")"
                                    }));
                                    break;
                                }
                                self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>()[i].SetSpell(genericSpell);
                                self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>()[i].SetVisibility(true);
                                self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>()[i].SetSelected(i < 4 + ModMain.Settings.BonusWizardPreparationSlots && spellLevel == UIGrimoireManager.Instance.LevelButtons.CurrentLevel);
                                self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>()[i].SetDisabled(GameState.InCombat || !UIGrimoireManager.Instance.CanEditGrimoire);
                                i++;
                            }
                        }
                    }
                    while (i < self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>().Count)
                    {
                        self.Field("m_Spells").GetValue<List<UIGrimoireSpell>>()[i].SetVisibility(false);
                        i++;
                    }

                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Grimoire))]
        [HarmonyPatch("Start")]
        static class Grimoire_Start_Patch
        {
            static bool Prefix(Grimoire __instance)
            {
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {
                    int maxSpellsPerLevel = Grimoire.MaxSpellsPerLevel + ModMain.Settings.BonusWizardPreparationSlots;

                    if (__instance.Spells.Length != Grimoire.MaxSpellLevel)
                    {
                        if (__instance.Spells.Length > Grimoire.MaxSpellLevel)
                        {
                            Debug.LogError("Too many spell levels in grimoire '" + __instance.name + "': some will be dropped!");
                        }
                        Grimoire.SpellChapter[] array = new Grimoire.SpellChapter[Grimoire.MaxSpellLevel];
                        __instance.Spells.CopyTo(array, 0);
                        __instance.Spells = array;
                    }
                    for (int i = 0; i < __instance.Spells.Length; i++)
                    {
                        if (__instance.Spells[i] == null)
                        {
                            __instance.Spells[i] = new Grimoire.SpellChapter();
                        }
                        else if (__instance.Spells[i].SpellData.Length != maxSpellsPerLevel)
                        {

                            if (__instance.Spells[i].SpellData.Length > maxSpellsPerLevel)
                            {
                                Debug.LogError(string.Concat(new object[]
                                {
                                    "Too many spell slots in grimoire '",
                                    __instance.name,
                                    "' for level ",
                                    i + 1,
                                    ": some will be dropped!"
                                }));
                            }


                            GenericSpell[] array2 = new GenericSpell[maxSpellsPerLevel];
                            for (int j = 0; j < Mathf.Min(array2.Length, __instance.Spells[i].SpellData.Length); j++)
                            {
                                array2[j] = __instance.Spells[i].SpellData[j];
                            }
                            __instance.Spells[i].SpellData = array2;
                        }
                    }

                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Grimoire))]
        [HarmonyPatch("HasSpell")]
        static class Grimoire_HasSpell_Patch
        {
            static void Postfix(ref bool __result, Grimoire __instance, GenericSpell spell)
            {
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {
                    if (spell != null)
                    {
                        int num = spell.SpellLevel - 1;
                        if (num >= 0 && num < 8)
                        {
                            for (int i = 4; i < 4 + ModMain.Settings.BonusWizardPreparationSlots; i++)
                            {
                                if (i <= __instance.Spells[num].SpellData.Length && __instance.Spells[num].SpellData[i] != null && __instance.Spells[num].SpellData[i].DisplayName.StringID == spell.DisplayName.StringID)
                                {
                                    __result = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Grimoire))]
        [HarmonyPatch("FindNewSpells")]
        static class Grimoire_FindNewSpells_Patch
        {
            static bool Prefix(Grimoire __instance, List<GenericSpell> newSpells, CharacterStats casterStats, int maxSpellLevel)
            {
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {
                    int maxSpellsPerLevel = Grimoire.MaxSpellsPerLevel + ModMain.Settings.BonusWizardPreparationSlots;

                    if (!casterStats)
                    {
                        return true;
                    }

                    int num = Mathf.Min(maxSpellLevel, Grimoire.MaxSpellLevel);
                    for (int i = 0; i < num; i++)
                    {
                        for (int j = 0; j < maxSpellsPerLevel; j++)
                        {
                            GenericSpell genericSpell = __instance.Spells[i].SpellData[j];
                            if (genericSpell != null)
                            {
                                bool flag = false;
                                for (int k = 0; k < newSpells.Count; k++)
                                {
                                    if (GenericAbility.NameComparer.Instance.Equals(newSpells[k], genericSpell))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    bool flag2 = false;
                                    foreach (GenericAbility genericAbility in casterStats.ActiveAbilities)
                                    {
                                        if (genericAbility is GenericSpell && genericSpell.DisplayName.StringID == genericAbility.DisplayName.StringID)
                                        {
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                    if (!flag2)
                                    {
                                        newSpells.Add(genericSpell);
                                    }
                                }
                            }
                        }
                    }

                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Grimoire.SpellChapter))]
        [HarmonyPatch("IsFull")]
        static class SpellChapter_IsFull_Patch
        {
            static void Postfix(ref bool __result, Grimoire.SpellChapter __instance)
            {
                if (ModMain.Settings.BonusWizardPreparationSlots > 0)
                {

                    int maxSpellsPerLevel = Grimoire.MaxSpellsPerLevel + ModMain.Settings.BonusWizardPreparationSlots;

                    __result = __instance.SpellData.Count((GenericSpell gs) => gs) >= maxSpellsPerLevel;
                }
            }
        }


    }
}
