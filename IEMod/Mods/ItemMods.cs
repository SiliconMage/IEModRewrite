using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class ItemMods
    {
        [HarmonyPatch(typeof(CampingSupplies))]
        [HarmonyPatch("StackMaximum", MethodType.Getter)]
        static class CampingSupplies_StackMaximum_Patch
        {
            static void Postfix(ref int __result, CampingSupplies __instance)
            {
                __result = ModMain.Settings.MaxCampingSupplies > 0 ? ModMain.Settings.MaxCampingSupplies : __result;
            }
        }

        [HarmonyPatch(typeof(ItemMod))]
        [HarmonyPatch("MaximumModValue", MethodType.Getter)]
        static class ItemMod_MaximumModValue_Patch
        {
            static void Postfix(ref int __result)
            {
                if (ModMain.Settings.ExtraItemModsValue > 0)
                {
                    __result += ModMain.Settings.ExtraItemModsValue;
                }
            }
        }
    }
}
