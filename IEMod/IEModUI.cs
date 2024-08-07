using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod
{
    public class IEModUI
    {
        public static IEModSettings Settings { get; set; }
        public static bool Enabled { get; set; }

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUIStyle toolTipStyle = new GUIStyle();
            toolTipStyle.richText = true;
            toolTipStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle explanationStyle = new GUIStyle();
            explanationStyle.richText = true;
            explanationStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyle colorStyle = new GUIStyle();
            colorStyle.richText = true;
            colorStyle.alignment = TextAnchor.LowerCenter;
            colorStyle.fontSize = 20;

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Console Commands Do Not Disable Achievements:", "<b><color=red>Please be responsible with this option!</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.CommandLineDoesNotDisableAchievements = GUILayout.Toggle(Settings.CommandLineDoesNotDisableAchievements,$" {Settings.CommandLineDoesNotDisableAchievements} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Disable Friendly Fire:","<b><color=cyan>Stops spells and attacks from affecting and hurting allies.  Areas of Effect will avoid neutral NPCs, but they will still turn hostile if attacked.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.DisableFriendlyFire = GUILayout.Toggle(Settings.DisableFriendlyFire, $" {Settings.DisableFriendlyFire} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Target Turned Enemies:","<b><color=cyan>Allows Area of Effects to affect turned allies and turned enemies.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.TargetTurnedEnemies = GUILayout.Toggle(Settings.TargetTurnedEnemies, $" {Settings.TargetTurnedEnemies} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Allow Combat Only Abilities to be used outside of combat:","<b><color=cyan>Removes the Combat Only restriction from all abilities.  Saving and reloading may be required when acquiring new abilities.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.AllowCombatOnlyAbilitiesToBeUsedAnytime = GUILayout.Toggle(Settings.AllowCombatOnlyAbilitiesToBeUsedAnytime, $" {Settings.AllowCombatOnlyAbilitiesToBeUsedAnytime} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Group Druid, Priest and Wizard spells on toolbar:","<b><color=cyan>Combines Druid, Priest and Wizard spells into the spell icon toolbar for whichever class the character is, while hiding the spell from the ability bar.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.GroupDruidPriestWizardSpellsOnToolbar = GUILayout.Toggle(Settings.GroupDruidPriestWizardSpellsOnToolbar, $" {Settings.GroupDruidPriestWizardSpellsOnToolbar} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Bonus Wizard Preparation Slots in Grimoire:","<b><color=cyan>Adds the specified amount of bonus preparation slots to Wizard grimoires.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.BonusWizardPreparationSlots = (int)GUILayout.HorizontalSlider(Settings.BonusWizardPreparationSlots,0f,3f,GUILayout.Width(150f));
            GUILayout.Label($" {Settings.BonusWizardPreparationSlots } ", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Enable Fast Scouting:", "<b><color=cyan>Allows the party to scout at a faster speed.  Use the 'B' key to cycle between faster scouting and slower scouting. This will conflict with any in-game keybinds you might have configured.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.EnableFastScouting = GUILayout.Toggle(Settings.EnableFastScouting, $" {Settings.EnableFastScouting} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Enable Walk Speed:","<b><color=cyan>Allows the party to move at a slower pace.  Use the 'W' key to cycle between running and walking.  This will conflict with any in-game keybinds you might have configured.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.EnableWalkSpeed = GUILayout.Toggle(Settings.EnableWalkSpeed, $" {Settings.EnableWalkSpeed} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Selection Circle Color:", "<b><color=cyan>Sets the color of the selection circle beneath of friendly and neutral characters.  This setting is not compatible with Color Blind mode.  Changing colors mid game will require exiting to the main menu and loading your game.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.SelectionCircleColor = (int)GUILayout.HorizontalSlider(Settings.SelectionCircleColor,0f,7f,GUILayout.Width(150f));
            GUILayout.Space(5);
            GUILayout.Label($"<b><color={Settings.ColorToHex(Settings.ColorList[Settings.SelectionCircleColor])}>{Settings.ColorLabelOptions[Settings.SelectionCircleColor]}</color></b>",colorStyle,GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(8);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Enable Ability Modifications:","<b><color=cyan>Loads changes from the abilityMods.json file and applies them to abilities in the game.  Changes may have to be reloaded after a new ability is acquired.  Changes can be reapplied by saving and loading the game.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.LoadAbilityMods = GUILayout.Toggle(Settings.LoadAbilityMods, $" {Settings.LoadAbilityMods} ",GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Enable Bonus Spell Slots:","<b><color=cyan>Calculates bonus spell slots based off of the caster's Intellect.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.CalcBonusSpellSlots = GUILayout.Toggle(Settings.CalcBonusSpellSlots, $" {Settings.CalcBonusSpellSlots} ",GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Increase XP Required to Level:","<b><color=cyan>Increases the amount of XP required for a character to level.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.IncreaseXPToLevel = (int)GUILayout.HorizontalSlider(Settings.IncreaseXPToLevel,0f,3f,GUILayout.Width(150f));
            GUILayout.Space(5);
            GUILayout.Label($"{Settings.XPOptionsLabelList[Settings.IncreaseXPToLevel]}",GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Unlock Restricted Equipment Slots:","<b><color=cyan>Unlocks Pet equipment slots for Party Members and Helmet equipment slots for Godlike.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.UnlockEquipmentSlots = GUILayout.Toggle(Settings.UnlockEquipmentSlots, $" {Settings.UnlockEquipmentSlots} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Replace Backer Names:","<b><color=cyan>Changes the names of Backer NPCs in the game.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.BackerNamesOption = (int)GUILayout.HorizontalSlider(Settings.BackerNamesOption,0f,2f,GUILayout.Width(150f));
            GUILayout.Space(5);
            GUILayout.Label($"{Settings.BackerNamesLabelList[Settings.BackerNamesOption]}",GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Disable Backer Dialog:","<b><color=cyan>Prevents Backer dialog from displaying when a Backer NPC is clicked.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.DisableBackerDialog = GUILayout.Toggle(Settings.DisableBackerDialog, $" {Settings.DisableBackerDialog} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Auto Save Behavior:","<b><color=cyan>Changes auto save behavior for the game.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.AutoSaveOption = (int)GUILayout.HorizontalSlider(Settings.AutoSaveOption, 0f, 2f, GUILayout.Width(150f));
            GUILayout.Space(5);
            GUILayout.Label($"{Settings.AutoSaveLabelList[Settings.AutoSaveOption]}", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Enable Companion Stats Modifications:","<b><color=cyan>Loads changes to companion and main character stats from the companionStats.json file.  Changes may need to be reapplied by saving and loading the game after acquiring a new companion for the first time.</color></b>"), GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.EnableCompanionStatModifications = GUILayout.Toggle(Settings.EnableCompanionStatModifications, $" {Settings.EnableCompanionStatModifications} ",GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Play Game Audio When Window Loses Focus:","<b><color=cyan>Lets the game audio play in the background when the application isn't in focus.</color></b>"),GUILayout.Width(400f));
            GUILayout.Space(10);
            Settings.PlayAudioWhenWindowLosesFocus = GUILayout.Toggle(Settings.PlayAudioWhenWindowLosesFocus, $" {Settings.PlayAudioWhenWindowLosesFocus} ", GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("<b><color=white>____________________</color></b>",explanationStyle,GUILayout.MaxWidth(1920),GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Box(GUI.tooltip, toolTipStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndHorizontal();

        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
