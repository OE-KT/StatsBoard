using stats.Models;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace stats.Behaviours
{
    internal class Statsboard : MonoBehaviour
    {
        internal static Statsboard Instance;

        internal Board board;
        private Sprite ProfilePicture;

        private void Start()
        {
            Instance = this;

            board = new Board
                (
                transform.Find("stat/Name").gameObject.GetComponent<Text>(),
                transform.Find("stat/RGB").GetComponent<Text>(),
                transform.Find("stat/tags").GetComponent<Text>(),
                transform.Find("stat/taged").GetComponent<Text>(),
                transform.Find("stat/matches").GetComponent<Text>(),
                transform.Find("stat/colorImg").GetComponent<Image>(),
                transform.Find("stat/pfp").GetComponent<Image>(),
                transform.Find("stat/Time played S").GetComponent<Text>(),
                transform.Find("stat/Time played T").GetComponent<Text>(),
                transform.Find("stat/date").GetComponent<Text>(),
                transform.Find("stat/Time").GetComponent<Text>()
            );
            //ProfilePicture = Main.LoadPNG(BepInEx.Paths.PluginPath + "/statspfp.png"); // Changed the path, so...
            string pfpLocation = BepInEx.Paths.PluginPath + "/statspfp.png";
            if (File.Exists(pfpLocation))
                board.pfp.sprite = Main.LoadPNG(pfpLocation);
            else
                Main.Instance.manualLogSource.LogWarning("No profile picture found at " + pfpLocation);

            // Positioning

            transform.position = new Vector3(-62.8345f, 12.334f, -83.214f);
            transform.rotation = Quaternion.Euler(0.1f, 180f, 0.1f);
            transform.localScale = new Vector3(0.0224f, 0.0248f, 0.0269f);

            if (Main.Instance.AutoSave.Value)
                InvokeRepeating(nameof(AutoSave), 0, Main.Instance.AutoSaveInterval.Value);
            if (Main.Instance.AutoRefreshBoard.Value)
                InvokeRepeating(nameof(RefreshBoard), 0, Main.Instance.AutoRefreshBoardInterval.Value);

            RefreshBoard();
        }

        private void AutoSave()
        {
            Main.Instance.manualLogSource.LogInfo("Auto saving statistics...");
            StatisticsController.SaveData();
        }

        internal void RefreshBoard()
        {
            if (!(board is object))
                throw new System.Exception("Board is null");
            Main.Instance.manualLogSource.LogMessage("Refreshing board...");

            //var LocalMaterial = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0];
            var data = Main.Instance.Data;

            board.name.text = PlayerPrefs.GetString("playerName").ToUpper();
            board.tags.text = $"TAGS: {data.Tags}";
            board.tagged.text = $"TIMES TAGGED: {data.Tagged}";
            board.matches.text = $"HUNT MATCHES WON: ({data.huntwins}/{data.HuntPlayed})";

            // Not totally sure what the point of all of this is, so I am just to leave it in
            float R = PlayerPrefs.GetFloat("redValue") / 1.0f * 255.0f;
            float G = PlayerPrefs.GetFloat("greenValue") / 1.0f * 255.0f;
            float B = PlayerPrefs.GetFloat("blueValue") / 1.0f * 255.0f;
            byte R_ = Convert.ToByte(R);
            byte G_ = Convert.ToByte(G);
            byte B_ = Convert.ToByte(B);
            board.RGBcoler.text = $"R: {PlayerPrefs.GetFloat("redValue") / 1.0f * 255.0f} G: {PlayerPrefs.GetFloat("greenValue") / 1.0f * 255.0f} G: {PlayerPrefs.GetFloat("blueValue") / 1.0f * 255.0f}";
            board.colorimg.color = new Color32(R_, G_, B_, 255);
        }

        internal void RefreshTime()
        {
            if (!(board is object))
                throw new System.Exception("Board is null");

            var data = Main.Instance.Data;

            TimeSpan SessionTime = TimeSpan.FromSeconds(CalculateTimePlayed.GetTimePlayed());
            TimeSpan TotalTime = TimeSpan.FromSeconds(data.TodleTimne);
            board.Timeplayedsessoin.text = $"TIME PLAYED (SESSION): {SessionTime.Hours}:{SessionTime.Minutes}:{SessionTime.Seconds}";
            board.timeplayedLifetieme.text = $"TIME PLAYED (LIFETIME): {TotalTime.Hours}:{TotalTime.Minutes}:{TotalTime.Seconds}";
            // SATURDAY, DECEMBER 31, 9999
            board.date.text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            board.timeNow.text = DateTime.Now.ToString("hh:mm:ss tt");
        }
    }
}
