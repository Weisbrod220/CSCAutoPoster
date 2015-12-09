using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerBeacon.Facebook.Graph;
using ComputerBeacon.Facebook.Server;
using Facebook;
using Newtonsoft.Json;
using SharedDependency;

namespace FacebookPost
{
    public class Facebook : IPoster
    {
        private string Page { get; }
        FacebookClient FacebookClient { get; }

        //Facebook authorization 
        public Facebook(string key, string page)
        {
            
            FacebookClient = new FacebookClient(key);
            Page = page;
        }

        //Facebook post method
        public void Post(string message)
        {
            try
            {
                Console.WriteLine("Posting to facebook wall:" + message);
                var request = User.CreateStatus("1693691520914941", FacebookClient.AccessToken, message);
                var response = request.GetResponse();
                Console.WriteLine("Successfully posted FB post as:" + response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
