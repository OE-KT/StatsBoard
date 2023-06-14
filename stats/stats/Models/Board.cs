using UnityEngine.UI;

namespace stats.Models
{
    internal class Board
    {
        internal Text name;
        internal Text timeplayedLifetieme;
        internal Text Timeplayedsessoin;
        internal Text RGBcoler;
        internal Text tags;
        internal Text tagged;
        internal Text matches;
        internal Image pfp;
        internal Image colorimg;
        internal Text date;
        internal Text timeNow;

        internal Board(Text name, Text rGBcoler, Text tags, Text tagged, Text matches, Image colorimg, Image pfp, Text timeplayedsessoin, Text timeplayedLifetieme, Text date, Text timeNow)
        {
            this.name = name;
            this.timeplayedLifetieme = timeplayedLifetieme;
            Timeplayedsessoin = timeplayedsessoin;
            RGBcoler = rGBcoler;
            this.tags = tags;
            this.tagged = tagged;
            this.matches = matches;
            this.pfp = pfp;
            this.colorimg = colorimg;
            this.date = date;
            this.timeNow = timeNow;
        }
    }
}
