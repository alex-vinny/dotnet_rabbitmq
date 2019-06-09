using System;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new SomeSubscriber();
            //subscriber.Start();

            var client = new SomeRequester();
            client.Start();
        }
    }
}
