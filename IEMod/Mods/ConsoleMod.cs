using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace IEMod.Mods
{
    internal class ConsoleMod
    {
        [HarmonyPatch(typeof(CommandLine))]
        [HarmonyPatch("IRoll20s", MethodType.Normal)]
        static class CommandLine_IRoll20s_Patched
        {
            static bool Prefix()
            {
                GameState.Instance.CheatsEnabled = !GameState.Instance.CheatsEnabled;
                if (GameState.Instance.CheatsEnabled && AchievementTracker.Instance != null && !ModMain.Settings.CommandLineDoesNotDisableAchievements)
                {
                    AchievementTracker.Instance.DisableAchievements = true;
                }
                if (GameState.Instance.CheatsEnabled)
                {
                    global::Console.AddMessage("Cheats Enabled");

                    if (AchievementTracker.Instance.DisableAchievements)
                    {
                        global::Console.AddMessage("Achievements have been disabled for this play-through.", Color.red);
                    }

                    return false;
                }

                global::Console.AddMessage("Cheats Disabled");

                return false;
            }
        }

        [HarmonyPatch(typeof(CommandLineRun))]
        [HarmonyPatch("RunCommand", MethodType.Normal)]
        static class CommandLineRun_RunCommand_Patched
        {
            static bool Prefix(string command)
            {

                if (string.IsNullOrEmpty(command))
                {
                    return false;
                }

                if (command.ToLower() == "runcommand")
                {
                    return false;
                }

                IList<string> list = StringUtility.CommandLineStyleSplit(command);
                string str = "";

                foreach (var methodInfo in typeof(NewConsoleMethods).GetMethods())
                {
                    if (string.Compare(methodInfo.Name, list[0], true) == 0)
                    {
                        object[] parameters;
                        
                        if (CommandLineRun.FillMethodParams(methodInfo, list, out parameters, out str))
                        {
                            methodInfo.Invoke(null, parameters);
                            return false;
                        }
                        
                        
                        global::Console.AddMessage("Unable to execute method '" + methodInfo.Name + "'.",Color.yellow);
                        return false;
                    }
                }

                return true;
            }
        }


        public class NewConsoleMethods
        {
            [Cheat]
            public static void CheckAchievementStatus()
            {
                if (AchievementTracker.Instance.DisableAchievements)
                {
                    global::Console.AddMessage("Achievements are disabled!", Color.red);
                }
                else
                {
                    global::Console.AddMessage("Achievements are enabled!", Color.green);
                }

            }

            [Cheat]
            public static void EnableAchievements()
            {
                if (AchievementTracker.Instance.DisableAchievements)
                {
                    Traverse.Create(AchievementTracker.Instance).Field("m_disableAchievements").SetValue(false);

                    if (!AchievementTracker.Instance.DisableAchievements)
                    {
                        global::Console.AddMessage("Achievements have been enabled again!",Color.green);
                    }
                }
                else
                {
                    global::Console.AddMessage("Achievements are already enabled.", Color.cyan);
                }
            }

            public static void WaitHours(string hours)
            {

                int numHoursToWait = 0;

                try
                {
                    numHoursToWait = int.Parse(hours);
                }
                catch (Exception e)
                {
                    global::Console.AddMessage("Cannot execute command WaitHours.  Valid integer parameter required.", Color.red);
                    numHoursToWait = -1;
                }


                if (ModMain.Settings.AdvanceStrongholdTimeWithCommands && WorldTime.Instance.CurrentTime != null &&
                                        numHoursToWait > 0)
                {
                    WorldTime.Instance.AdvanceTimeByHours(numHoursToWait, false);
                }
                else if (WorldTime.Instance.CurrentTime != null && numHoursToWait > 0)
                {
                    WorldTime.Instance.CurrentTime.AddHours(numHoursToWait);
                }
            }

            public static void WaitDays(string days)
            {
                int numDaysToWait = 0;

                try
                {
                    numDaysToWait = int.Parse(days);
                }
                catch (Exception e)
                {
                    global::Console.AddMessage("Cannot execute command WaitDays.  Valid integer parameter required.", Color.red);
                    numDaysToWait = -1;
                }

                if (ModMain.Settings.AdvanceStrongholdTimeWithCommands && WorldTime.Instance.CurrentTime != null &&
                    numDaysToWait > 0)
                {
                    WorldTime.Instance.AdvanceTimeByHours(numDaysToWait * WorldTime.Instance.HoursPerDay, false);
                }
                else if (WorldTime.Instance.CurrentTime != null && numDaysToWait > 0)
                {
                    WorldTime.Instance.CurrentTime.AddDays(numDaysToWait);
                }

               
            }
        }
    }
}
