using System;
using UnityEngine;

namespace stats.Behaviours
{
    internal class CalculateTimePlayed : MonoBehaviour
    {
        internal static CalculateTimePlayed Instance;

        private int _time;
        private int TotalTimePlayedInSession;

        private void Awake()
        {
            Instance = this;
        }
        private void Update()
        {
            // A very primative counter, I may change it in the future
            if (DateTime.Now.Second != _time)
            {
                _time = DateTime.Now.Second;
                TotalTimePlayedInSession++;
                Main.Instance.Data.TodleTimne++;
                Statsboard.Instance.RefreshTime();
            }
        }

        /* Static methods */

        /// <summary>
        /// Returns the time played in seconds (session)
        /// </summary>
        internal static int GetTimePlayed()
        {
            return Instance.TotalTimePlayedInSession;
        }
    }
}
