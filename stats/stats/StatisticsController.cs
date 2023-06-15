/*
    I am reusing this code from the orginal because I want to ensure it is able to decode players old saves
*/
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace stats
{
    public static class StatisticsController
    {
        public static void SaveData()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.streamingAssetsPath + "/Stas.kt";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            statsData data = Main.Instance.Data;

            formatter.Serialize(fileStream, data);
            fileStream.Close();

            Main.Instance.manualLogSource.LogInfo("SAVED BANANA FIEND DATA TO " + path);
        }
        public static statsData LoadPlayer()
        {
            string path = Application.streamingAssetsPath + "/Stas.kt";
            if (File.Exists(path))
                return GetDataFromPath();
            else
            {
                Debug.Log("No savefile found :( path  was " + path);

                Main.Instance.Data = new statsData();
                SaveData();
                return Main.Instance.Data;
            }

            statsData GetDataFromPath()
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                statsData data = binaryFormatter.Deserialize(fileStream) as statsData;
                fileStream.Close();

                return data;
            }
        }
        public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
