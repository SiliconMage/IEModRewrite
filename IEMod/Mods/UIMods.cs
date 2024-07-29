using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class UIMods
    {
        
        [HarmonyPatch(typeof(UIAbilityBarButtonSet))]
        [HarmonyPatch("SetButtonsAnticlassSpells", MethodType.Normal)]
        static class UIAbilityBarButtonSet_SetButtonsAnticlassSpells_Patch
        {
            public static bool Prefix(UIAbilityBarButtonSet __instance, CharacterStats stats)
            {

                if (ModMain.Settings.GroupDruidPriestWizardSpellsOnToolbar)
                {
                    Traverse.Create(__instance).Method("SetIdentification",new Type[]{typeof(string),typeof(string)},new object[] {null,null});

                    Func<GenericAbility, bool> action = delegate(GenericAbility ability)
                    {
                        GenericSpell genericSpell = ability as GenericSpell;
                        return genericSpell && ability.MasteryLevel == 0 &&
                               genericSpell.StatusEffectGrantingSpell == null &&
                               (!IsGroupedClass(genericSpell) ||
                                !CharacterStats.IsPlayableClass(genericSpell.SpellClass)) &&
                               !GenericAbility.AbilityTypeIsAnyEquipment(ability.EffectType);
                    };

                    int startButton = Traverse.Create(__instance).Method("AddAbilityButtons",
                        new Type[]
                        {
                            typeof(CharacterStats), typeof(Func<GenericAbility, bool>),
                            typeof(UIAbilityBarButtonSet.AbilityButtonAction), typeof(int)
                        },
                        new object[] {stats, action, UIAbilityBarButtonSet.AbilityButtonAction.CAST_SPELL_ABILITY, 0}).GetValue<int>();


                    Traverse.Create(__instance).Method("HideButtons", new Type[] { typeof(int) }, new object[] { startButton });
                    Traverse.Create(__instance).Field("m_DoRefresh").SetValue(true);

                    return false;
                }

                return true;
            }

            public static bool IsGroupedClass(GenericSpell spell)
            {
                switch (spell.SpellClass)
                {
                    case CharacterStats.Class.Druid:
                    case CharacterStats.Class.Priest:
                    case CharacterStats.Class.Wizard:
                        return true;
                }

                return false;
            }
        }
        

        [HarmonyPatch(typeof(UIAbilityBarButtonSet))]
        [HarmonyPatch("ShowOnSpellBar")]
        static class UIAbiiltyBarButtonSet_ShowOnSpellBar_Patch
        {
            static void Postfix(ref bool __result, GenericAbility ability, CharacterStats stats, int spellLevel)
            {
                if (ability is GenericSpell)
                {
                    var spell = ability as GenericSpell;

                    if ((spell.SpellClass == CharacterStats.Class.Druid ||
                        spell.SpellClass == CharacterStats.Class.Priest ||
                        spell.SpellClass == CharacterStats.Class.Wizard) &&
                        spell.SpellLevel == spellLevel)
                    {

                        if (spell.SpellClass == CharacterStats.Class.Wizard &&
                            ability.WhyNotReady == GenericAbility.NotReadyValue.NotInGrimoire)
                        {
                            return;
                        }

                        switch (stats.CharacterClass)
                        {
                            case CharacterStats.Class.Druid:
                            case CharacterStats.Class.Priest:
                            case CharacterStats.Class.Wizard:
                                __result = true;
                                break;
                        }
                    }
                }
            }
        }
    }
}
