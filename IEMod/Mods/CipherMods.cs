using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class CipherMods
    {
        [HarmonyPatch(typeof(FocusTrait))]
        [HarmonyPatch("StartingFocus",MethodType.Getter)]
        static class FocusTrait_StartingFocus_Patch
        {
            public enum CipherFocusOptions
            {
                Default = 0,
                None = 1,
                Half = 2,
                ThreeFourths = 3,
                Full = 4
            }

            static void Postfix(ref float __result, FocusTrait __instance)
            {
                if (ModMain.Settings.CipherFocusOption > (int)CipherFocusOptions.Default)
                {

                    float baseMaxFocus = __instance.MaxFocus - __instance.MaxFocusBonus;

                    switch (ModMain.Settings.CipherFocusOption)
                    {
                        case (int)CipherFocusOptions.None:
                            __result = 0;
                            break;
                        case (int)CipherFocusOptions.Half:
                            __result = baseMaxFocus / 2f;
                            break;
                        case (int)CipherFocusOptions.ThreeFourths:
                            __result = baseMaxFocus * 3f / 4f;
                            break;
                        case (int)CipherFocusOptions.Full:
                            __result = baseMaxFocus;
                            break;
                    }

                    __result += __instance.MaxFocusBonus;
                }
            }
        }
    }
}
