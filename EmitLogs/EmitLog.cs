using System;
using System.Text;
using RabbitMQ.Client;

namespace EmitLogs
{
    class EmitLog
    {
        public static void Run(string[] args)
        {
            Console.WriteLine("Start NewTask:");

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    string message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);   

                    channel.BasicPublish(exchange: "logs",
                        routingKey: "",
                        mandatory: false,
                        basicProperties: null,
                        body: body
                    );

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ?
                string.Join(" ", args) :
                "Hello World");
        }
    }
}
