using MargieBot;
using MargieBot.Models;
using SharedDependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SlackBotLibrary
{
    public class Slack : IPoster
    {
        Bot myBot;
        //Creating a new bot
        public Slack(string token)
        {
            myBot = new Bot();
            myBot.Connect(token);
        }

        //Slack post method
        public void Post(string message)
        {
            while (myBot.IsConnected == false)
            {
                Task.Delay(100);
            }
            foreach (var channel in myBot?.ConnectedGroups)
            {
                if (!channel.Name.Contains("rss"))
                    return;
                Console.WriteLine($"Posting to slack channel: {channel.Name} with message: {message}");
                var botMessage = new BotMessage();
                botMessage.Text = message;
                botMessage.ChatHub = channel;
                Console.WriteLine($"Successfully posted to channel: {channel.Name} with message: {message}");
                myBot.Say(botMessage);
            }
        }
    }
}
