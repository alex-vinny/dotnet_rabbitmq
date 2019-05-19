using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiveLogs
{
    public class ReceiveLog
    {
        public static void Run(string[] args)
        {            
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                        exchange: "logs",
                        routingKey: "");

                    Console.WriteLine(" [*] Waiting for logs.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);                        
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
