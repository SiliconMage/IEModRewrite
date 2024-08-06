using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

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
    }
}
