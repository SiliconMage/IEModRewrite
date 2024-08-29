using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class DetectionMods
    {
        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("DetectionRange")]
        static class CharacterStats_DetectionRange_Patch
        {
            static void Postfix(float __result, CharacterStats __instance)
            {
                if (ModMain.Settings.DisableNonStealthDetectionPenalty &&
                    !Stealth.IsInStealthMode(__instance.gameObject))
                {
                    __result += 4f;
                }
            }
        }
    }
}
