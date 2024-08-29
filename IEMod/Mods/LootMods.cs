using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class LootMods
    {
        [HarmonyPatch(typeof(Loot))]
        [HarmonyPatch("SetSeed")]
        static class Loot_SetSeed_Mod
        {
            static void Postfix()
            {
                if (!ModMain.Settings.EnableLootShuffler)
                {
                    return;
                }

                UnityEngine.Random.InitState(Environment.TickCount);
            }
        }
    }
}
