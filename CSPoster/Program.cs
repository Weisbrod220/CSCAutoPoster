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

namespace CSPoster
{
    class Program
    {
        private static PosterClass Posters { get; set; }
        private static bool isRunning = true;

        static void Main(string[] args)
        {
            Posters = new PosterClass();
            var rssLinks = new List<string>
            {
                "http://www.nytimes.com/services/xml/rss/nyt/International.xml"
            };
            var rss = new RSSFeeds.RSSFeeds(rssLinks);
            rss.rssFeedProcd += PostNewRSS;
            rss.ForcePostRSS();
            while (isRunning)
            {
                var command = Console.ReadLine();
                if (command == "quit")
                    isRunning = false;
                else if(command == "post")
                    rss.ForcePostRSS();
            }
        }

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
            var twitterPoster = new Twitter("9iFxZVBAxChzgeOLZhboFANmx", "RbrXziSt2RuzbCQWMjXC5RMrTH9itTeZdw4RcqWYjmQtvP6mpk");

            var facebookPoster = new Facebook("CAAM3ZCVBzx90BAPPu1ZBAYXLjzUdXmtZBmxFDDjwxVzYLTVUmG1J6oaoE6DcO28cf4hZBTZBo71QEMnZC2lB0oXsEZC5zfr5mDl8qmEAQmQhbxTvZAI3I0SFN60FcqSOk6ReAVYLToiuIQAA5zxhNFqNpZAJZCZAZAOwpU8fKJDZBZAYIQs049zMZB0ltFh9Vb6k2ZBy3GImU2N5VZCgjewZDZD", "1693691520914941");
            Posters.Add(twitterPoster);
            Posters.Add(facebookPoster);
        }

        public void Post(string message)
        {
            foreach (var poster in Posters)
            {
                poster.Post(message);
            }
        }
    }
}
