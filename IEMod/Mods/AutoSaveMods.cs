using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class AutoSaveMods
    {
        public enum AutoSaveOptions
        {
            Original = 0,
            SaveBeforeTransition = 1,
            Disabled = 2
        }

        [HarmonyPatch]
        static class GameState_ChangeLevel_Patch
        {
            private static MethodBase TargetMethod() => AccessTools.Method(typeof(GameState),"ChangeLevel",new Type[] {typeof(MapData)});

            static bool Prefix(GameState __instance, MapData map)
            {
                if (ModMain.Settings.AutoSaveOption == (int)AutoSaveOptions.SaveBeforeTransition)
                {
                    GameState.Autosave();
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(GameState))]
        [HarmonyPatch("FinalizeLevelLoad")]
        static class GameState_FinalizeLevelLoad_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
            {
                if (ModMain.Settings.AutoSaveOption != (int)AutoSaveOptions.Original &&
                    (ModMain.Settings.AutoSaveOption == (int)AutoSaveOptions.SaveBeforeTransition ||
                    ModMain.Settings.AutoSaveOption == (int)AutoSaveOptions.Disabled))
                {

                    int callAutoSaveIndex = -1;
                    List<CodeInstruction> codes = new List<CodeInstruction>(codeInstructions);

                    for (int i = 0; i < codes.Count; i++)
                    {
                        if (codes[i].opcode == OpCodes.Call)
                        {
                            string operand = codes[i].operand.ToString();

                            if (!string.IsNullOrWhiteSpace(operand) && operand.Contains("Autosave"))
                            {
                                callAutoSaveIndex = i;
                                break;
                            }
                        }
                    }

                    if (callAutoSaveIndex >= 0)
                    {
                        codes[callAutoSaveIndex].opcode = OpCodes.Nop;
                    }


                    return codes.AsEnumerable();
                }

                return codeInstructions;
            }
        }
    }
}
