using System;
using Messages;
using EasyNetQ;
using EasyNetQ.Logging;

namespace Subscriber
{
    public class SomeRequester
    {
        public void Start()
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);
            
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var input ="";
                Console.WriteLine("Simple Client application.");
                Console.WriteLine("Enter a message or press 'q' key to quit.");
                while ((input = Console.ReadLine()) != "q")
                {
                    var myRequest = new TextRequest { Text = input };

                    var task = bus.RequestAsync<TextRequest, TextResponse>(myRequest);
                    task.ContinueWith(response =>
                    {
                        Console.WriteLine("Got async response: \"{0}\"", response.Result.Text);
                    });

                    //var response = bus.Request<TextRequest, TextResponse>(myRequest);                    
                    //Console.WriteLine("Response for server: \"{0}\"", response.Text);
                }
            }
        }
    }
}