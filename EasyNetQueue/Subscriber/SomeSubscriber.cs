using System;
using Messages;
using EasyNetQ;
using EasyNetQ.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SomeSubscriber
    {
         public void Start()
         {
            LogProvider.SetCurrentLogProvider(ConsoleLogProvider.Instance);
            
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                //bus.Subscribe<TextMessage>("test", HandleTextMessage);

                //bus.SubscribeAsync<TextMessage>("subscribe_async", message =>                
                //    new WebClient().DownloadStringTaskAsync(new Uri(message.Text))
                //        .ContinueWith(task =>
                //            Console.WriteLine("Received: '{0}', Downloaded: '{1}'", message.Text, task.Result)));
                
                bus.SubscribeAsync<TextMessage>("long_running_queue", 
                    message => Task.Factory.StartNew(() => 
                    {
                        // Perform some critical actions here
                        // If some exception raise, the continuation will handle that
                        
                        Task.Delay(2000); // long running process

                        if(message.Text == "error")
                            throw new Exception("Something goes wrong here!!!");
                        
                        Console.WriteLine("Received: '{0}'", message.Text);

                        Task.Delay(1000); // long running process

                    }).ContinueWith(task =>
                    {
                        if(task.IsCompleted && !task.IsFaulted)
                        {
                            // Every thing is ok                            
                            Console.Clear();
                        }
                        else
                        {
                            // Sent to default error on broker
                            throw new EasyNetQException("Message processing exception - look in the default error queue (broker).");
                        }
                    }));

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }

        private void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got message: {0}", textMessage.Text);
            Console.ResetColor();
        }
    }
}
