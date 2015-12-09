using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FacebookPost;
using RSSFeeds;
using SharedDependency;
using TwitterLibrary;
using SlackBotLibrary;
using MargieBot;

namespace CSPoster
{
    class Program
    {
        private static PosterClass Posters { get; set; }
        private static bool isRunning = true;

        static void Main(string[] args)
        {
            Posters = new PosterClass();
            //List of RSS feeds, can be added to and deleted from
            var rssLinks = new List<string>
            {
            "http://news.mit.edu/rss/feed",
            "http://feeds.webservice.techradar.com/us/rss/news/computing",
            "http://feeds.webservice.techradar.com/us/rss/news/software",
            "http://feeds.bbci.co.uk/news/technology/rss.xml?edition=uk",
            "http://www.computerweekly.com/rss/Latest-IT-news.xml",
            "http://www.computerweekly.com/rss/IT-security.xml",
            "http://www.computerweekly.com/rss/IT-careers-and-IT-skills.xml",
            "http://feeds.webservice.techradar.com/us/rss/news/internet",
            "http://feeds.webservice.techradar.com/us/rss/reviews/pc-mac",
            "http://feeds.webservice.techradar.com/us/rss/news/computing-components",
            "http://feeds.webservice.techradar.com/us/rss/news/networking"

            };
            var rss = new RSSFeeds.RSSFeeds(rssLinks);
            rss.rssFeedProcd += PostNewRSS;
            rss.ForcePostRSS();
            while (isRunning)
            {
                var command = Console.ReadLine();
                //quit program by typing quit into CMD
                if (command == "quit")
                    isRunning = false;
                //Force a post if you do not want to wait an hour by typing post into the CMD
                else if(command == "post")
                    rss.ForcePostRSS();
            }
        }

        //Gets the title and URL of the RSS item to post
        private static void PostNewRSS(object sender, RSSEvent e)
        {
            Posters.Post($"{e.Info.Title} : {e.Info.Uri}");
        }
    }

    public class PosterClass
    {
        private List<IPoster> Posters { get; } 


        public PosterClass()
        {
            Posters = new List<IPoster>();
            //Twitter client secret and key for posting to specific account, can be changed for any twitter account
            var twitterPoster = new Twitter("9iFxZVBAxChzgeOLZhboFANmx", "RbrXziSt2RuzbCQWMjXC5RMrTH9itTeZdw4RcqWYjmQtvP6mpk");
            //Facebook key and secret, can be changed to any facebook account
            var facebookPoster = new Facebook("CAAM3ZCVBzx90BAPPu1ZBAYXLjzUdXmtZBmxFDDjwxVzYLTVUmG1J6oaoE6DcO28cf4hZBTZBo71QEMnZC2lB0oXsEZC5zfr5mDl8qmEAQmQhbxTvZAI3I0SFN60FcqSOk6ReAVYLToiuIQAA5zxhNFqNpZAJZCZAZAOwpU8fKJDZBZAYIQs049zMZB0ltFh9Vb6k2ZBy3GImU2N5VZCgjewZDZD", "1693691520914941");
            var Slackbot = new Slack("xoxb-16248408758-qo2hl1MlbaNApbbwbY7fMN3P");

            //adding posters to the list 
            Posters.Add(Slackbot);
            Posters.Add(twitterPoster);
            Posters.Add(facebookPoster);

            
        }

        //interface for posting, used in every service thats needs to post
        public void Post(string message)
        {
            foreach (var poster in Posters)
            {
                poster.Post(message);
            }
        }
    }
}
