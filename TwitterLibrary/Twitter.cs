using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using SharedDependency;

namespace TwitterLibrary 
{
    public class Twitter : IPoster
    {
        private Tokens TwitterTokens { get; }
        //Twitter authorization 
        public Twitter(string key, string secret)
        {
            var session = OAuth.Authorize(key, secret);
            Console.WriteLine(session.AuthorizeUri);
            
            Console.WriteLine("Please enter twitter pin code.");
            var pin = Console.ReadLine();
            TwitterTokens = session.GetTokens(pin);
        }
        //Twitter Post method
        public void Post(string tweet)
        {
            try {
                Console.WriteLine($"Tweeting: {tweet}");
                var returnText = TwitterTokens.Statuses.Update(status => tweet);
                Console.WriteLine("Successfully posted tweet at:" + returnText.CreatedAt);
            }
           catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
