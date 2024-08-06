using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class SelectionCircleMods
    {
        [HarmonyPatch(typeof(InGameHUD))]
        [HarmonyPatch("FetchColors")]
        static class InGameHUD_FetchColors_Patch
        {
            static bool Prefix(InGameHUD __instance)
            {
                if (ModMain.Settings.SelectionCircleColor > 0)
                {

                    var self = Traverse.Create(__instance);

                    __instance.FriendlyColorBlind.color = ModMain.Settings.ColorList[ModMain.Settings.SelectionCircleColor];
                    self.Field<Color>("FriendlyColorColorBlind").Value = __instance.FriendlyColorBlind.color;
                    __instance.FriendlySelected.color = ModMain.Settings.ColorList[ModMain.Settings.SelectionCircleColor];
                    __instance.Friendly.color = ModMain.Settings.ColorList[ModMain.Settings.SelectionCircleColor];
                    self.Field<Color>("FriendlyColor").Value = __instance.FriendlyColorBlind.color;
                    self.Field<Color>("FoeColor").Value = __instance.FoeMaterial.color;


                    return false;
                }

                return true;
            }
        }
    }
}
