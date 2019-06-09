using System;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var somePub = new SomePublisher();
            //somePub.Start();

            var server = new SomeResponder();
            server.Start();
        }
    }
}
