using System;
using Messages;
using EasyNetQ;
using EasyNetQ.Logging;
using System.Linq;

namespace Publisher
{
    public class SomeResponder
    {
        public void Start()
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);

            Console.WriteLine("Starting server ...");

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {

               bus.Respond<TextRequest, TextResponse>(request => 
                new TextResponse
                {
                    Text = new string(request.Text.Reverse().ToArray())
                });

               Console.WriteLine("Waiting for calls, to stop press 'enter'.");
               Console.ReadLine();
            }           
        }
    }
}
