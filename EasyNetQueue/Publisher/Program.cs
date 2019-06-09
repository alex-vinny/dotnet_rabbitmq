using System;
using Messages;
using EasyNetQ;
using EasyNetQ.Logging;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);

            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                var input ="";
                Console.WriteLine("Enter a message. 'Quit' to quit.");
                while ((input = Console.ReadLine()) != "Quit")
                {
                    bus.Publish(new TextMessage
                    {
                        Text = input
                    });
                }
            }

            Console.WriteLine("Done.");
        }
    }
}
