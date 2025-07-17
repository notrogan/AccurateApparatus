using BepInEx;
using BepInEx.Logging;
using AccurateApparatus.Patches;
using HarmonyLib;

namespace AccurateApparatus
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class AccurateApparatusBase : BaseUnityPlugin
    {
        private const string modGUID = "rogan.AccurateApparatus";
        private const string modName = "Accurate Apparatus";
        private const string modVersion = "3.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static AccurateApparatusBase Instance;

        internal static ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("AccurateApparatus Started!");

            harmony.PatchAll(typeof(AccurateApparatusBase));
            harmony.PatchAll(typeof(PlayerControllerPatch));
        }
    }
}
