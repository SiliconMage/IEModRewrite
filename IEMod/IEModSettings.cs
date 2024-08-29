using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod
{
    public class IEModSettings : UnityModManager.ModSettings
    {
        public bool CommandLineDoesNotDisableAchievements = false;
        public bool DisableFriendlyFire = false;
        public bool TargetTurnedEnemies = false;
        public bool AllowCombatOnlyAbilitiesToBeUsedAnytime = false;
        public bool GroupDruidPriestWizardSpellsOnToolbar = false;
        public int BonusWizardPreparationSlots = 0;
        public bool EnableFastScouting = false;
        public bool FastScoutToggle = false;
        public bool EnableWalkSpeed = false;
        public bool WalkToggle = false;
        public int SelectionCircleColor = 0;
        [XmlIgnore]
        public List<Color> ColorList = new List<Color>() { Color.green, Color.yellow, Color.blue, Color.cyan, Color.magenta, Color.white, new Color(1f,0.6471f,0,1f), new Color(0.5020f,0f,0.5020f)};
        [XmlIgnore]
        public List<string> ColorLabelOptions = new List<string>() {"Default", "Yellow", "Blue", "Cyan", "Magenta", "White", "Orange", "Purple"};
        public bool LoadAbilityMods = false;
        public bool CalcBonusSpellSlots = false;
        public int IncreaseXPToLevel = 0;
        [XmlIgnore]
        public List<string> XPOptionsLabelList = new List<string>(){"Default","25% Increase","50% Increase", "Square progression"};
        public bool UnlockEquipmentSlots = false;
        public int BackerNamesOption = 0;
        [XmlIgnore]
        public List<string> BackerNamesLabelList = new List<string>() { "No Modifications", "Fantasy Names", "Race Only" };
        public bool DisableBackerDialog = false;
        public int AutoSaveOption = 0;
        [XmlIgnore]
        public List<string> AutoSaveLabelList = new List<string>(){"Default", "Auto Save Before Transition","Disable Autosave"};
        public bool EnableCompanionStatModifications = false;
        public bool PlayAudioWhenWindowLosesFocus = false;
        public bool DisableNonStealthDetectionPenalty = false;
        public int MaxCampingSupplies = 0;
        public bool EnablePerEncounterSpells = false;
        public bool EnableLootShuffler = false;
        public bool AdvanceStrongholdTimeWithCommands = false;


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public string ColorToHex(Color c)
        {
            int red = (int)(255f * c.r);
            int green = (int)(255f * c.g);
            int blue = (int)(255f * c.b);

            return $"#{red.ToString("X2")}{green.ToString("X2")}{blue.ToString("X2")}ff";
        }
    }
}
