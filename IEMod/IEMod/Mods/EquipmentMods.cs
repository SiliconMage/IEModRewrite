using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class EquipmentMods
    {
        [HarmonyPatch(typeof(Equipment))]
        [HarmonyPatch("HasEquipmentSlot")]
        static class Equipment_HasEquipmentSlot_Patch
        {
            static void Postfix(ref bool __result, Equipment __instance, Equippable.EquipmentSlot slot)
            {
                if (ModMain.Settings.UnlockEquipmentSlots)
                {
                    CharacterStats charStats = __instance.GetComponent<CharacterStats>();

                    if (slot == Equippable.EquipmentSlot.Pet && !__result)
                    {
                        __result = true;
                    }

                    if (slot == Equippable.EquipmentSlot.Head && charStats.CharacterRace == CharacterStats.Race.Godlike && !__result)
                    {
                        __result = true;
                    }
                }
            }
        }

    }
}
