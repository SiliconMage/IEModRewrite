using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace IEMod.Mods
{
    internal class VolumeMods
    {
        [HarmonyPatch(typeof(VolumeAsCategory))]
        [HarmonyPatch("UpdateVolume")]
        static class VolumeAsCategory_UpdateVolumn_Patch
        {
            static void Postfix(VolumeAsCategory __instance)
            {
                if (ModMain.Settings.PlayAudioWhenWindowLosesFocus)
                {
                    var self = Traverse.Create(__instance);

                    if (__instance.Source)
                    {
                        __instance.Source.volume = 1f;
                    }
                }
            }
        }
    }
}
