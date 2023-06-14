namespace stats
{
    [System.Serializable]
    public class statsData
    {
        public int Tags;
        /// <summary>
        /// The amount of times you have been tagged
        /// </summary>
        public int Tagged;
        public int huntwins;
        public double TodleTimne;

        public statsData()
        {
        }
        public statsData(int Tags, int Tagged, int HuntWin, int TotalTime)
        {
            this.Tags = Tags;
            this.Tagged = Tagged;
            huntwins = HuntWin;
            TodleTimne = TotalTime;
        }
    }
}
