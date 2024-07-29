using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class XPMods
    {
        public enum XPOptions
        {
            Disabled = 0,
            Increase25Perc = 1,
            Increase50Perc = 2,
            Squared = 3
        }


        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("ExperienceNeededForLevel")]
        static class CharacterStats_ExperienceNeededForLevel_Patch
        {
            static void Postfix(ref int __result, int level)
            {
                if (ModMain.Settings.IncreaseXPToLevel > 0)
                {
                    switch (ModMain.Settings.IncreaseXPToLevel)
                    {
                        case (int)XPOptions.Increase25Perc:
                            __result = (level - 1) * level * 625;
                            break;
                        case (int)XPOptions.Increase50Perc:
                            __result = (level - 1) * level * 750;
                            break;
                        case (int)XPOptions.Squared:
                            __result = (level - 1) * level * 1000;
                            break;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("ExperienceNeededForNextLevel")]
        static class CharacterStats_ExperiencedNeededForNextLevel_Patch
        {
            static void Postfix(ref int __result, int currentLevel)
            {
                if (ModMain.Settings.IncreaseXPToLevel > 0)
                {
                    switch (ModMain.Settings.IncreaseXPToLevel)
                    {
                        case (int)XPOptions.Increase25Perc:
                            __result = currentLevel * (currentLevel + 1) * 625;
                            break;
                        case (int)XPOptions.Increase50Perc:
                            __result = currentLevel * (currentLevel + 1) * 750;
                            break;
                        case (int)XPOptions.Squared:
                            __result = currentLevel * (currentLevel + 1) * 1000;
                            break;
                    }
                }
            }
        }
    }
}
