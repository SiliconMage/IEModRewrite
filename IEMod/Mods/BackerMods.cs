using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class BackerMods
    {
        public enum BackerModOptions
        {
            Original = 0,
            FantasyNames = 1,
            RaceOnly = 2
        }

        [HarmonyPatch(typeof(GameState))]
        [HarmonyPatch("FinalizeLevelLoad")]
        public static class GameState_FinalizeLevelLoad_Patch
        {

            private static FantasyNames fantasyNameLists;

            static void Postfix(GameState __instance)
            {
                if (ModMain.Settings.BackerNamesOption != (int)BackerModOptions.Original)
                {
                    GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
                    List<GameObject> allBackers = new List<GameObject>();

                    for (int i = allObjects.Length - 1; i > 0; i--)
                    {
                        if ((allObjects[i].name.StartsWith("NPC_BACKER") || allObjects[i].name.StartsWith("NPC_Visceris"))
                            && allObjects[i].GetComponent<CharacterStats>() != null)
                        {
                            allBackers.Add(allObjects[i]);
                        }
                    }


                    switch (ModMain.Settings.BackerNamesOption) 
                    {
                        case (int)BackerModOptions.FantasyNames:
                        {
                            if (fantasyNameLists == null)
                            {
                                fantasyNameLists = InitializeNames();
                            }

                            if (fantasyNameLists != null)
                            {

                                foreach (GameObject backer in allBackers)
                                {
                                    if (ModMain.Settings.DisableBackerDialog)
                                    {
                                        GameUtilities.Destroy(backer.GetComponent<NPCDialogue>());
                                    }

                                    CharacterStats backerStats = backer.GetComponent<CharacterStats>();
                                    ReplaceBackerName(backerStats);
                                }
                            }

                            break;
                        }
                        case (int)BackerModOptions.RaceOnly:
                        {
                            foreach (GameObject backer in allBackers)
                            {
                                if (ModMain.Settings.DisableBackerDialog)
                                {
                                    GameUtilities.Destroy(backer.GetComponent<NPCDialogue>());
                                }

                                CharacterStats backerStats = backer.GetComponent<CharacterStats>();
                                ReplaceBackerName(backerStats, true);
                            }

                            break;
                        }
                    }
                }
            }

            private static void ReplaceBackerName(CharacterStats backer, bool replaceWithRaceName = false)
            {

                if (replaceWithRaceName)
                {
                    backer.OverrideName = GUIUtils.GetRaceString(backer.CharacterRace, backer.Gender);
                    return;
                }

                int randomSeed = backer.DisplayName.ToString().GetHashCode();
                UnityEngine.Random.InitState(randomSeed);
                int maxNumNames = 0;
                int nameIndex = 0;

                switch (backer.CharacterRace)
                {
                    case CharacterStats.Race.Human:
                    {
                        maxNumNames = backer.Gender == Gender.Female ? fantasyNameLists.Human_F.Length : fantasyNameLists.Human_M.Length;
                        nameIndex = UnityEngine.Random.Range(0, maxNumNames);
                        backer.OverrideName = backer.Gender == Gender.Female ? fantasyNameLists.Human_F[nameIndex] : fantasyNameLists.Human_M[nameIndex];
                        break;
                    }
                    case CharacterStats.Race.Elf:
                    {
                        maxNumNames = backer.Gender == Gender.Female ? fantasyNameLists.Elf_F.Length : fantasyNameLists.Elf_M.Length;
                        nameIndex = UnityEngine.Random.Range(0, maxNumNames);
                        backer.OverrideName = backer.Gender == Gender.Female ? fantasyNameLists.Elf_F[nameIndex] : fantasyNameLists.Elf_M[nameIndex];
                        break;
                    }
                    case CharacterStats.Race.Dwarf:
                    {
                        maxNumNames = backer.Gender == Gender.Female ? fantasyNameLists.Dwarf_F.Length : fantasyNameLists.Dwarf_M.Length;
                        nameIndex = UnityEngine.Random.Range(0, maxNumNames);
                        backer.OverrideName = backer.Gender == Gender.Female ? fantasyNameLists.Dwarf_F[nameIndex] : fantasyNameLists.Dwarf_M[nameIndex];
                        break;
                    }
                    case CharacterStats.Race.Orlan:
                    {
                        maxNumNames = backer.Gender == Gender.Female ? fantasyNameLists.Orlan_F.Length : fantasyNameLists.Orlan_M.Length;
                        nameIndex = UnityEngine.Random.Range(0, maxNumNames);
                        backer.OverrideName = backer.Gender == Gender.Female ? fantasyNameLists.Orlan_F[nameIndex] : fantasyNameLists.Orlan_M[nameIndex];
                        break;
                    }
                    case CharacterStats.Race.Aumaua:
                    {
                        maxNumNames = backer.Gender == Gender.Female ? fantasyNameLists.Aumaua_F.Length : fantasyNameLists.Aumaua_M.Length;
                        nameIndex = UnityEngine.Random.Range(0, maxNumNames);
                        backer.OverrideName = backer.Gender == Gender.Female ? fantasyNameLists.Aumaua_F[nameIndex] : fantasyNameLists.Aumaua_M[nameIndex];
                        break;
                    }
                }
            }

            private static FantasyNames InitializeNames()
            {
                FantasyNames retValue = null;

                DirectoryInfo appDataDir = new DirectoryInfo(Application.dataPath);
                DirectoryInfo modsDir = new DirectoryInfo(Path.Combine(appDataDir.Parent.FullName, $"Mods{Path.DirectorySeparatorChar}IEMod"));

                if (modsDir.Exists)
                {
                    FileInfo fantasyNamesFile = new FileInfo($"{modsDir.FullName}{Path.DirectorySeparatorChar}fantasyNames.json");

                    if (fantasyNamesFile.Exists)
                    {
                        if (fantasyNameLists == null)
                        {
                            try
                            {
                                UnityModManager.Logger.Log("Reading and initializing fantasy names...");
                                retValue = JsonConvert.DeserializeObject<FantasyNames>(File.ReadAllText(fantasyNamesFile.FullName));
                                UnityModManager.Logger.Log("Initialization complete!");
                            }
                            catch (Exception e)
                            {
                                UnityModManager.Logger.Log("An error occurred while trying to read file fantasyNames.json.");
                                UnityModManager.Logger.Log($"Exception: {e.Message}");

                                retValue = null;
                            }
                        }
                    }
                }

                return retValue;
            }
        }

        public class FantasyNames
        {
            public string[] Aumaua_F { get; set; }
            public string[] Aumaua_M { get; set; }
            public string[] Dwarf_F { get; set; }
            public string[] Dwarf_M { get; set; }
            public string[] Elf_F { get; set; }
            public string[] Elf_M { get; set; }
            public string[] Human_F { get; set; }
            public string[] Human_M { get; set; }
            public string[] Orlan_F { get; set; }
            public string[] Orlan_M { get; set; }

        }
    }
}
