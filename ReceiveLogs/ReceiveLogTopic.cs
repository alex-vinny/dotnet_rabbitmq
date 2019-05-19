using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiveLogs
{
    public class ReceiveLogTopic
    {
        public static void Run(string[] args)
        {            
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                    
                    var queueName = channel.QueueDeclare().QueueName;

                    if(args.Length < 1)
                    {
                        Console.Error.WriteLine("Usage: {0} [binding key...]",
                            Environment.GetCommandLineArgs()[0]);
                        
                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                        Environment.ExitCode = 1;
                        return;
                    }

                    foreach (var severity in args)
                    {
                        channel.QueueBind(queue: queueName,
                            exchange: "topic_logs",
                            routingKey: severity);
                    }

                    Console.WriteLine(" [*] Waiting for messages. To exit press [enter].");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;

                        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);                        
                    };

                    channel.BasicConsume(queue:queueName,
                        autoAck: true,
                        consumer: consumer
                    );

                    Console.WriteLine("# => To exit press ENTER");
                    Console.ReadLine();
                }
            }
        }
    }
}
