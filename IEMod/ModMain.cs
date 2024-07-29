using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityModManagerNet;

namespace IEMod
{
    static class ModMain
    {
        public static IEModSettings Settings;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Settings = IEModSettings.Load<IEModSettings>(modEntry);
            IEModUI.Settings = Settings;

            modEntry.OnGUI = IEModUI.OnGUI;
            modEntry.OnToggle = IEModUI.OnToggle;
            modEntry.OnSaveGUI = IEModUI.OnSaveGUI;

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }
    }
}
