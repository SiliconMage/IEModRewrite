using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace IEMod.Mods
{
    internal class DisableFriendlyFireMod
    {
        [HarmonyPatch(typeof(AttackAOE))]
        [HarmonyPatch("FindAoeTargets", MethodType.Normal)]
        static class AttackAOE_FindAoeTargets_Patch
        {
            static void Postfix(ref List<GameObject> __result, AttackAOE __instance, GameObject caster, Vector3 parentForward, Vector3 hitPosition, bool forUi)
            {
                List<GameObject> filteredList = new List<GameObject>();
                Faction casterFaction = caster.GetComponent<Faction>();

                foreach (var target in __result)
                {
                    Faction targetFaction = target.GetComponent<Faction>();

                    if (ModMain.Settings.DisableFriendlyFire && __instance.ValidTargets == AttackBase.TargetType.All)
                    {
                        if (!FriendlyRightNowAndAlsoWhenConfused(target, caster) && !NeutralRightNow(target,caster))
                        {
                            filteredList.Add(target);
                        }

                        if (HostileEvenIfConfused(target, caster))
                        {
                            filteredList.Add(target);
                        }
                    }
                    else
                    {
                        filteredList.Add(target);
                    }
                }

                __result = filteredList;
            }
        }

        [HarmonyPatch(typeof(Trap))]
        [HarmonyPatch("CanActivate", MethodType.Normal)]
        static class Trap_CanActivate_Patch
        {
            static void Postfix(ref bool __result, Trap __instance, GameObject victim)
            {
                if (ModMain.Settings.DisableFriendlyFire)
                {
                    var ownerFaction = __instance.Owner?.GetComponent<Faction>();
                    var victimFaction = victim.GetComponent<Faction>();

                    if (ownerFaction != null && ownerFaction.IsFriendly(victim) || NeutralRightNow(victim,__instance.Owner))
                    {
                        __result = false;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CharacterStats))]
        [HarmonyPatch("ComputeHitAdjustment", MethodType.Normal)]
        static class CharacterStats_ComputeHitAdjustments_Patch
        {
            static bool Prefix(CharacterStats __instance, int hitValue, CharacterStats enemyStats, DamageInfo damage)
            {
                if (ModMain.Settings.DisableFriendlyFire)
                {

                    if (FriendlyRightNowAndAlsoWhenConfused(enemyStats.gameObject, __instance.gameObject) || NeutralRightNow(enemyStats.gameObject,__instance.gameObject))
                    {
                        damage.IsMiss = true;
                        damage.Interrupts = false;
                        damage.IsCriticalHit = false;
                        damage.IsGraze = false;
                        damage.IsKillingBlow = false;
                        damage.DamageMult(0f);

                        return false;
                    }

                }

                return true;
            }
        }


        public static bool FriendlyRightNowAndAlsoWhenConfused(GameObject target, GameObject caster)
        {
            var targetFaction = target?.GetComponent<Faction>();
            var casterFaction = caster?.GetComponent<Faction>();
            var targetAiController = GameUtilities.FindActiveAIController(target);
            if (ModMain.Settings.TargetTurnedEnemies && targetAiController != null && casterFaction != null)
            {
                var targetOriginallyFriendly = targetAiController.GetOriginalTeam()?.GetRelationship(casterFaction.CurrentTeam)
                                               == Faction.Relationship.Friendly;
                if (!targetOriginallyFriendly)
                {
                    return false;
                }
            }
            return targetFaction?.IsFriendly(caster) == true || casterFaction?.IsFriendly(target) == true;
        }

        public static bool NeutralRightNow(GameObject target, GameObject caster)
        {
            var targetFaction = target?.GetComponent<Faction>();
            var casterFaction = caster?.GetComponent<Faction>();

            if (casterFaction == null || targetFaction == null)
            {
                return false;
            }
            
            return targetFaction.CurrentTeam.GetRelationship(casterFaction.CurrentTeam) == Faction.Relationship.Neutral;
        }

        public static bool HostileEvenIfConfused(GameObject target, GameObject caster)
        {
            var targetFaction = target?.GetComponent<Faction>();
            var casterFaction = caster?.GetComponent<Faction>();
            var targetAiController = GameUtilities.FindActiveAIController(target);
            if (targetFaction?.IsHostile(caster) == true || casterFaction?.IsHostile(target) == true)
            {
                //they're actually hostile
                return true;
            }
            if (targetAiController == null)
            {
                //not AI controller. Can never be confused.
                return false;
            }
            if (!ModMain.Settings.TargetTurnedEnemies)
            {
                //no more checks if the option is disabled.
                return false;
            }

            var targetOriginal = targetAiController.GetOriginalTeam()?.GetRelationship(casterFaction?.CurrentTeam)
                                 == Faction.Relationship.Hostile;

            return targetOriginal;
        }

    }
}
