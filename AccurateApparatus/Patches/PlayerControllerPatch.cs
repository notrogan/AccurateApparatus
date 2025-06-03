using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace AccurateApparatus.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerPatch
    {
        public static float CoroutineTime = 0f;
        public static PlayerControllerB NetworkController;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void HoldDamagePatch(ref PlayerControllerB __instance)
        {
            if (__instance.currentlyHeldObjectServer != null)
            {
                if (__instance.currentlyHeldObjectServer.name.ToString() == "LungApparatus(Clone)" || __instance.currentlyHeldObjectServer.name.ToString() == "LungApparatusTurnedOff(Clone)")
                {
                    CoroutineTime += Time.deltaTime;

                    if (CoroutineTime > 1f)
                    {
                        if (__instance.health < 10)
                        {
                            __instance.bleedingHeavily = true;
                        }

                        if (__instance.health > 0 && __instance.health < 20)
                        {
                            __instance.health -= 2;
                        }
                        else if (__instance.health > 0)
                        {
                            __instance.health -= 1;
                        }
                        else
                        {
                            __instance.DamagePlayer(int.MaxValue, true, true, CauseOfDeath.Unknown, 0, false, default);
                        }

                        HUDManager.Instance.UpdateHealthUI(__instance.health, false);

                        HUDManager.Instance.UIAudio.Stop();
                        HUDManager.Instance.HUDAnimator.ResetTrigger("SmallHit");
                        HUDManager.Instance.HUDAnimator.ResetTrigger("CriticalHit");

                        CoroutineTime = 0f;
                    }
                }
            }
        }
    }
}