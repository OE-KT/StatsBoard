using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace stats
{
    [BepInPlugin(ID, NAME, VERSION)]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            ID = "com.kt.gorillatag.stats",
            NAME = "stats",
            VERSION = "1.0.1";

        internal static Main Instance;

        internal statsData Data;
        internal ManualLogSource manualLogSource => Logger;
        // internal bool InRoom;

        internal ConfigEntry<bool> AutoSave;
        internal ConfigEntry<float> AutoSaveInterval;
        internal ConfigEntry<bool> AutoRefreshBoard;
        internal ConfigEntry<float> AutoRefreshBoardInterval;

        internal Main()
        {
            Instance = this;
            manualLogSource.LogInfo($"Loaded! v{VERSION}");

            AutoSave = Config.Bind("AutoSave", "Enabled", true, "Automatically save data every x seconds");
            AutoSaveInterval = Config.Bind("AutoSave", "Interval", 15f, "How often to save data (in seconds)");
            AutoRefreshBoard = Config.Bind("AutoRefreshBoard", "Enabled", false, "Automatically refresh the board every x seconds");
            AutoRefreshBoardInterval = Config.Bind("AutoRefreshBoard", "Interval", 5f, "How often to refresh the board (in seconds)");

            Data = StatisticsController.LoadPlayer();
            // Handle invalid data
            if (Data.HuntPlayed < Data.huntwins)
            {
                manualLogSource.LogWarning("Invalid data detected! Looks like this is a new stat data");
                Data.HuntPlayed = Data.huntwins;
            }

            new HarmonyLib.Harmony(ID).PatchAll();
        }

        internal async void GameInitialized()
        {
            manualLogSource.LogInfo($"Attempting to load all assets!");
            Instantiate(await LoadAsset("stats")).AddComponent<Behaviours.Statsboard>();

            new GameObject("Time Calculations").AddComponent<Behaviours.CalculateTimePlayed>();
            new GameObject("Photon Callbacks").AddComponent<Behaviours.Callbacks>();
        }

        /* Utilities */

        private AssetBundle _assetBundle;
        internal async Task<GameObject> LoadAsset(string Name)
        {
            if (!(_assetBundle is object))
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("stats.Resources.stat"))
                {
                    manualLogSource.LogMessage("Assetbundle not found, loading from resources");
                    AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromStreamAsync(stream);
                    new WaitUntil(() => assetBundleCreateRequest.isDone);
                    _assetBundle = assetBundleCreateRequest.assetBundle;
                }
            AssetBundleRequest assetBundleRequest = _assetBundle.LoadAssetAsync<GameObject>(Name);
            new WaitUntil(() => assetBundleRequest.isDone);
            return assetBundleRequest.asset as GameObject;
        }

        internal static Sprite LoadPNG(string filePath)
        {
            Texture2D tex = null;
            Sprite sprite = null;
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            }
            return sprite;
        }
    }
}
