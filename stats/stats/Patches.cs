using HarmonyLib;

namespace stats
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPatch(typeof(GorillaLocomotion.Player), "Awake"), HarmonyPostfix]
        private static void GorillaLocomotion_Player_Awake()
        {
            // Due to the latest update the assetbundle cannot be loaded on init (for da mod)
            Main.Instance.GameInitialized();
        }

        /*[HarmonyPatch(typeof(GorillaNetworking.GorillaComputer), "OnJoinedRoom"), HarmonyPostfix, HarmonyWrapSafe]
        private static void GorillaNetworking_GorillaComputer_OnJoinedRoom()
        {
            Main.Instance.IsLocalTagged = false;
        }*/

        [HarmonyPatch(typeof(VRRig), "PlayTagSound"), HarmonyPrefix, HarmonyWrapSafe]
        private static void HandTagSound(int soundIndex, float soundVolume) // The soundIndex2 check is from BananaHook
        {
            if (soundIndex == 2)
            {
                if (GorillaGameManager.instance is GorillaHuntManager && GorillaTagger.Instance.offlineVRRig.currentMatIndex != 0)
                {
                    Main.Instance.manualLogSource.LogMessage("You won hunt:) Great job");
                    Main.Instance.Data.huntwins++;
                    Behaviours.Statsboard.Instance.RefreshBoard();
                }
                Main.Instance.IsLocalTagged = false;
            }
        }
    }
}


/*
    Unfortantly this is master only:/ So we will have to have a less efficent method for | Edit just moved to callbacks
*/
// https://harmony.pardeike.net/articles/patching-edgecases.html

/*[HarmonyPatch(typeof(GorillaTagManager), nameof(GorillaTagManager.ReportTag)), HarmonyPrefix, HarmonyWrapSafe]
private static void ReportTag(GorillaTagManager __instance, Player taggedPlayer, Player taggingPlayer)
{
    Main.Instance.manualLogSource.LogInfo($"Tag detected {taggingPlayer.NickName} just tagged {taggedPlayer.NickName}");

    // This code if ugly af -_- I may go back and refactor it later but I am in a rush atm
    if (taggedPlayer.IsLocal)
    {
        Main.Instance.manualLogSource.LogInfo("You fucking idiot, imagen getting tagged. Get fucking better");
        Main.Instance.Data.Tags++;
        Main.Instance.IsLocalTagged = true;
        Behaviours.Statsboard.Instance.RefreshBoard();
    }
    else if (taggingPlayer.IsLocal)
    {
        Main.Instance.manualLogSource.LogMessage("YOU DID IT:))))))))) Great job your one of the cool kids now");
        Main.Instance.Data.Tagged++;
        Main.Instance.IsLocalTagged = false;
        Behaviours.Statsboard.Instance.RefreshBoard();
    }
}*/