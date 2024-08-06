using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.Achievement;
using AI.Player;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class MovementMods
    {
        [HarmonyPatch(typeof(PartyMemberAI))]
        [HarmonyPatch("Update")]
        static class PartyMemberAI_Update_Patch
        {
            static void Postfix(PartyMemberAI __instance)
            {
                var self = Traverse.Create(__instance);

                if (ModMain.Settings.EnableFastScouting && ModMain.Settings.FastScoutToggle)
                {
                    if (self.Field<Mover>("m_mover").Value != null && Stealth.IsInStealthMode(__instance.gameObject))
                    {
                        float newValue = 4f;
                        self.Field<Mover>("m_mover").Value.UseCustomSpeed(newValue);
                    }
                }

                if (ModMain.Settings.EnableWalkSpeed && ModMain.Settings.WalkToggle)
                {
                    if (self.Field<Mover>("m_mover").Value != null && !Stealth.IsInStealthMode(__instance.gameObject) && !GameState.InCombat)
                    {
                        float walkSpeed = self.Field<Mover>("m_mover").Value.WalkSpeed;

                        self.Field<Mover>("m_mover").Value.UseCustomSpeed(walkSpeed);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UnityModManager.UI))]
        [HarmonyPatch("Update")]
        static class UnityModManager_UI_Update_Patch
        {
            private static void Postfix(UnityModManager.UI __instance, ref Rect ___mWindowRect, ref Vector2[] ___mScrollPosition, ref int ___tabId)
            {
                if (ModMain.Settings.EnableWalkSpeed && !GameInput.DisableInput && GameInput.GetKeyDown(KeyCode.W))
                {
                    ModMain.Settings.WalkToggle = !ModMain.Settings.WalkToggle;
                    return;
                }

                if (ModMain.Settings.EnableFastScouting && !GameInput.DisableInput && GameInput.GetKeyDown(KeyCode.B))
                {
                    ModMain.Settings.FastScoutToggle = !ModMain.Settings.FastScoutToggle;
                    return;
                }
            }
        }
    }
}
