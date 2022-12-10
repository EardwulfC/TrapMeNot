using BepInEx;

using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace TrapMeNot
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class TrapMeNot : BaseUnityPlugin
    {
        public const string PluginGuid = "Eardwulf.TrapMeNot";
        public const string PluginName = "TrapMeNot";
        public const string PluginVersion = "1.0.0";

        Harmony _harmony;

        public void Awake()
        {
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
        }

        [HarmonyPatch(typeof(Trap))]
        static class TrapPatch
        { 
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Trap.OnTriggerEnter))]
            static void OnTriggerEnterUpdate(ref Trap __instance)
            {
                if(__instance.m_triggeredByPlayers)
                {
                    if (!Player.m_localPlayer.m_pvp)
                    {
                        __instance.m_triggeredByPlayers = false;
                    }
                }
            }
        }


        public void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}