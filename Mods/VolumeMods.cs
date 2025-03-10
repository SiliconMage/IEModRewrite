using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityModManagerNet;

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


                    if (__instance.Source && !self.Field<bool>("m_hasFocus").Value)
                    {
                        __instance.Source.volume = __instance.ExternalVolume * GameState.Option.GetVolume(__instance.Category);
                    }
                }
            }
        }
    }
}
