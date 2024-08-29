using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class SpellSlotsMod
    {
        [HarmonyPatch(typeof(SpellMax))]
        [HarmonyPatch("GetSpellCastMax")]
        static class SpellMax_GetSpellCastMax_Patch
        {
            static void Postfix(ref int __result, GameObject caster, int spellLevel)
            {
                if (ModMain.Settings.CalcBonusSpellSlots && __result < int.MaxValue && __result > 0)
                {
                    var casterStats = caster.GetComponent<CharacterStats>();
                    var casterIntellect = casterStats.Intellect;
                    int bonusSpells = 0;

                    for (int i = 14 + spellLevel; i <= casterIntellect; i += 4)
                    {
                        ++bonusSpells;
                    }

                    __result += bonusSpells;
                }
            }
        }

        [HarmonyPatch(typeof(GenericSpell))]
        [HarmonyPatch("ActivateCooldown")]
        static class GenericSpell_ActivateCooldown_Patch
        {
            static void Postfix(GenericSpell __instance)
            {
                if (ModMain.Settings.EnablePerEncounterSpells && !GameState.InCombat)
                {

                    var self = Traverse.Create(__instance);

                    self.Field<CharacterStats>("m_ownerStats").Value.SpellCastCount[__instance.SpellLevel - 1]--;
                }
            }
        }

        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("HandleGameUtilitiesOnCombatEnd")]
        static class CharacterStats_HandleGameUtilitiesOnCombatEnd_Patch
        {
            static void Postfix(CharacterStats __instance, object sender, EventArgs e)
            {
                if (ModMain.Settings.EnablePerEncounterSpells)
                {
                    for (int i = 0; i < __instance.SpellCastCount.Length; ++i)
                    {
                        __instance.SpellCastCount[i] = 0;
                    }
                }
            }
        }
    }
}
