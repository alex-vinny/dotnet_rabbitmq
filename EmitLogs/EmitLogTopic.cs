using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace EmitLogs
{
    class EmitLogTopic
    {
        public static void Run(string[] args)
        {
            Console.WriteLine("Topic Log");

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

                    var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                    var message = (args.Length > 1)
                                ? string.Join(" ", args.Skip(1).ToArray())
                                : "Hello World";

                    var body = Encoding.UTF8.GetBytes(message);   

                    channel.BasicPublish(exchange: "topic_logs",
                        routingKey: routingKey,
                        mandatory: false,
                        basicProperties: null,
                        body: body
                    );

                    Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
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