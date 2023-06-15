/*
    Just to clarify this script is not messing with anything, it is simply reading the event data. 
    It is not sending anything.
*/
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace stats.Behaviours
{
    internal class Callbacks : MonoBehaviour, IOnEventCallback
    {
        private void Awake()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        // Due to the RPC target we cannot just use Harmony to patch the ReportTagRPC method
        public void OnEvent(EventData eventData)
        {
            // the try-catch statement is there in case the event has invalid arguments
            try
            {
                // Handle tag event sent
                if (eventData.Code == 1 || eventData.Code == 2 && eventData.CustomData is object[] && (eventData.CustomData as object[]).Length > 1)
                {
                    Main.Instance.manualLogSource.LogMessage("Got tag data!");

                    object[] SentData = eventData.CustomData as object[];
                    string taggingId = SentData[0] as string;
                    string taggedId = SentData[1] as string;

                    string LocalId = PhotonNetwork.LocalPlayer.UserId;
                    if (taggingId == LocalId)
                    {
                        Main.Instance.Data.Tags++;
                        Main.Instance.manualLogSource.LogInfo("Increased times tagging other");
                        Statsboard.Instance.RefreshBoard();
                    }
                    else if (taggedId == LocalId)
                    {
                        Main.Instance.Data.Tagged++;
                        Main.Instance.manualLogSource.LogInfo("Increated times tagged");
                        Statsboard.Instance.RefreshBoard();
                    }
                }
            }
            catch (System.Exception e)
            {
                Main.Instance.manualLogSource.LogError($"Error in OnEvent: {e}");
            }
        }
    }
}
