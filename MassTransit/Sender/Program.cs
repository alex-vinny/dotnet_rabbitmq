using System;
using Messages;
using MassTransit;

namespace MassSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            bus.Start();
            
            string input = "";

            Console.WriteLine("Press 'quit' to exit.");            
            while((input = Console.ReadLine()) != "quit")
            {
                bus.Publish(new TextMessage
                {
                    Text = input
                });
            }

            bus.Stop();
        }
    }
}
