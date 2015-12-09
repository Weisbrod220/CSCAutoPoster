using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using RSSFeeds.Properties;
using SimpleFeedReader;
using Timer = System.Timers.Timer;

namespace RSSFeeds
{
    public class RSSFeeds
    {
        private readonly List<string> _rssList;
        private List<Uri> postedLinks = new List<Uri>();
        Timer PostingTimer { get; }

        public event EventHandler<RSSEvent> rssFeedProcd;

        public RSSFeeds(List<string> rssList  )
        {
            _rssList = rssList;
            PostingTimer = new Timer(60000*60);
            //PostingTimer.Enabled = true;
            PostingTimer.Elapsed += PostMessage;
            PostingTimer.Start();
        }

        public void ForcePostRSS()
        {
            PostMessage(null, null);

        }

        private void PostMessage(object sender, ElapsedEventArgs e)
        {
            var maxCount = _rssList.Count();
            var r = new Random();
            var rssFeedToUse = r.Next(maxCount); 
       
            var reader = new FeedReader();
            var items = reader.RetrieveFeed(_rssList[rssFeedToUse]);

            foreach (var rss in items)
            {
                if (!postedLinks.Contains(rss.Uri))
                {
                    var info = new RSSInfo();
                    info.Uri = rss.Uri;
                    info.Title = rss.Title;
                    var eventInfo = new RSSEvent();
                    eventInfo.Info = info;
                    OnRSSReached(eventInfo);
                    postedLinks.Add(rss.Uri);
                    return;
                }
            }
            PostMessage(null, null);
        }

        protected virtual void OnRSSReached(RSSEvent e)
        {
            var handler = rssFeedProcd;
            handler?.Invoke(this, e);
        }

    }

    public class RSSEvent : EventArgs
    {
        public RSSInfo Info { get; set; }
    }

    public class RSSInfo
    {
        public Uri Uri { get; set; }
        public string Title { get; set; }
    }
}
