# IEModRewrite
The popular mod for Pillars of Eternity returns, written in the Unity Mod Manager framework! 


IE Mod started out as a collection of mods designed to bring Pillars of Eternity closer to old Infinity Engine games, such as Baldur's Gate and Icewind Dale.  It grew to include a hefty number of modifications, and fell by the wayside as Obsidian continued to update the game.  The old IE Mod is based on an older framework, the Patchwork framework, with which I was less familiar.  Seeing as how IE Mod is now out of date, I took the opportunity to pull in what functionality I could to rewrite IE Mod based on the Unity Mod Manager framework instead.

IE Mod: Rewrite contains customizations that allow for the following:


Console Commands Do Not Disable Achievements
Running console commands, particularly using the "IRoll20s" command disables achievements in a play-through, even if you are not cheating.  Enabling this option will prevent achievements from being disabled when using the IRoll20s command, but please, use this option responsibly! 

Additional commands include:

    CheckAchievementStatus: Tells you whether achievements are enabled in your playthrough.
    EnableAchievements: Forcefully enable achievements again in your playthrough.
    WaitHours: Takes an integer as an argument.  Advances time by the specified number of integer hours in the game.
    WaitDays: Takes an integer as an argument.  Advances time by the specified number of integer days in the game.


Disable Friendly Fire
Area of Effects will not target neutral or friendly NPCs, and attacks will automatically missing neutral or friendly NPCs.  Neutral NPCs will still turn hostile if attacked, however.


Target Turned Enemies
Area of Effects and attacks will be able to target charmed enemies and allies.

Allow Combat Only Abilities to be Used Outside of Combat
Combat only abilities can now be used outside of combat.

Group Druid, Priest and Wizard Spells on Toolbar
Makes it so that spells added to the player via console commands are grouped into the spell slot button of whichever class the character is.  So if a Wizard acquires a Priest spell, it will be accessible from the Wizard's spells on the toolbar for the respective level.

Bonus Wizard Preparation Slots in Grimoire
Adds a specified number of bonus preparation slots to the Wizard's grimoire.  Currently, the maximum is 3 so as not to break the UI.  Changing this option will require that you restart the game.

Enable Fast Scouting
Allows the party to scout at a faster speed.  Press the 'B' key to toggle between fast scouting and regular scouting speeds.

Enable Walk Speed
Allows the party to stroll around the map at a slower pace.  Press the 'W' key to toggle between running and walking.

Selection Circle Color
Sets the color for selection and highlight circles around non-hostile NPCs and party members.  This option is incompatible with color blind mode, however.

Enable Ability Modifications
Allows in-game abilities to be modified in the following ways:

     MinimumDamage - The lowest amount of damage the ability can deal
     MaximumDamage - The highest amount of damage the ability can deal
     AccuracyBonus - A bonus to accuracy when the ability is used
     Range - How far away from the target the ability can be used without having to move closer, or how far the ability's attack will travel
     DTBypass - An amount of damage from the ability that will ignore Damage Reduction
     Speed - How fast the ability executes.  Valid options are: 0,1,2.  These represent Instant, Short or Long
     Push - How far the targets of the ability are pushed on being hit
     DefendedBy - Which stat the ability is resisted by.  Valid options are: Deflect, Fortitude, Reflex, Will and None
     BlastRadius - Only for abilities with AoE.  The radius of the area of effect.  The bigger the radius, the more area the ability will affect.
     BlastAngle - The angle in degrees the AoE will affect. 360 is a full circle, while 180 is a half-circle.
     Modal - Whether or not the ability is a modal ability
     UseType - How often the ability recharges its uses.  Valid options are: None, PerEncounter, PerRest, Charged, PerStrongholdTurn
     Uses - The number of uses the ability has before it has to be recharged


This can be achieved by adding the ability to the abilityMods.json file in the mod's directory.  Please see the file abilityModInstructions.txt for an explanation and example of how to modify a specific ability in the game.


Enable Bonus Spell Slots
Calculates bonus spell casters for a caster based on the caster's Intellect.

Increase XP Required to Level
Increases the XP required to level by a certain amount.  The equations are based off of the old IE Mod and are as such:

Level      Original       25% Increase         50% Increase           Square Progression
1            0                  0                   0                        0
2           1000               1250                1500                     1000
3           3000               3750                4500                     4000
4           6000               7500                9000                     9000
5           10000             12500               15000                    16000
6           15000             18750               22500                    25000
7           21000             26250               31500                    36000
8           28000             35000               42000                    49000
9           36000             45000               54000                    64000
10          45000             56250               67500                    81000
11          55000             68750               82500                   100000
12          66000             82500               99000                   121000

Starting a new game with this option enabled is highly recommended.


Unlock Restricted Equipment Slots
Unlocks the Pet equipment slot for other party members and unlocks the Helmet equipment slot for Godlike.

Replace Backer Name
Changes the names of Backer NPCs in the game in the chosen way.  By default, the backers have their default name.  The "Fantasy Names" option will randomly choose a name from a list of fantasy names, which are contained in fantasyNames.json.  These values were compiled and taken from the original IE Mod.  The "Race Only" option will replace the name tag of the Backer NPC with just the race of their in game character model.

Disable Backer Dialog
This option prevents backer dialog from triggering if a Backer NPC is clicked.

Auto Save Behavior
Changes auto save behavior in the game.  By default, Auto Save functions as normal.  The "Auto Save Before Transition" option means that the game will autosave before transitioning to a new area, rather than after transitioning to a new area.  The "Disable Autosave" option will disable autosave from functioning entirely.

Enable Companion Stat Modifications
Allows companions, the main character and hired adventures to have their stats modified in the following ways:

    Might
    Constitution
    Dexterity
    Perception
    Intellect
    Resolve
    Athletics
    Lore
    Mechanics
    Stealth
    Survival


Modifying an attribute like "Constitution" or "Intellect" modifies the base value of the attribute, so modifiers will be added after the attribute's value is set from the JSON file.  Modifying a skill like "Lore" or "Mechanics" adds to the skill's bonus modifiers, which is computed after ranks and other bonus modifiers.  It will not appear on the Character Sheet as any type of bonus, but the number should still be increased by the amount specified in the JSON file.  Please see the file companionStatsInstructions.txt to see how to add entries to the companionStats.json file.
