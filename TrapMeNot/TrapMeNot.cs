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
            static bool OnTriggerEnterPrefix(ref Trap __instance, ref Collider collider)
            {
                if (__instance.m_triggeredByPlayers)
                {
                    Player player = collider.GetComponentInParent<Player>();

                    if (player && !player.IsPVPEnabled())
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}