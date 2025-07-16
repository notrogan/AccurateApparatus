using GameNetcodeStuff;
using HarmonyLib;
using System;
using UnityEngine;

namespace AccurateApparatus.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerPatch
    {
        public static PlayerControllerB NetworkController;

        public static int ClientId;
        public static string PlayerUsername;

        public static int Health = 0; 
        public static float Timer = 0f;

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void HoldDamagePatch(ref PlayerControllerB __instance)
        {
            if (GameNetworkManager.Instance.localPlayerController != null && GameNetworkManager.Instance.localPlayerController != null)
            {
                NetworkController = GameNetworkManager.Instance.localPlayerController;

                PlayerUsername = NetworkController.playerUsername;
                ClientId = (int)NetworkController.playerClientId;

                if (!NetworkController.isPlayerDead)
                {
                    if (StartOfRound.Instance.inShipPhase == false)
                    {
                        if (__instance.playerUsername == PlayerUsername)
                        {
                            if (__instance.currentlyHeldObjectServer != null)
                            {
                                if (__instance.currentlyHeldObjectServer.name.ToString() == "LungApparatus(Clone)" || __instance.currentlyHeldObjectServer.name.ToString() == "LungApparatusTurnedOff(Clone)")
                                {
                                    Timer += Time.fixedDeltaTime / 2.05f;

                                    if (Timer > 1.7f)
                                    {
                                        Health = __instance.health;
                                        AccurateApparatusBase.mls.LogInfo(Health);

                                        if (Health == 0)
                                        {
                                            __instance.DamagePlayer(int.MaxValue, true, true, CauseOfDeath.Unknown, 0, false, default);
                                            return;
                                        }

                                        if (Health < 10)
                                        {
                                            __instance.bleedingHeavily = true;
                                        } 
                                        
                                        if (Health > 0 && Health <= 20)
                                        {
                                            __instance.health -= 3;
                                        }
                                        else
                                        {
                                            __instance.health -= 1;
                                        }

                                        Timer = 0f;

                                        HUDManager.Instance.UIAudio.Stop();
                                        HUDManager.Instance.HUDAnimator.ResetTrigger("SmallHit");
                                        HUDManager.Instance.HUDAnimator.ResetTrigger("CriticalHit");
                                        HUDManager.Instance.UpdateHealthUI(__instance.health, false);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        __instance.bleedingHeavily = false;
                    }
                }
            }
        }
    }
}