using System;
using System.Threading;

namespace PressScaleServiceV4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pressFlow = new PressScaleFlow();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Beëindigen...");
                pressFlow.Stop();
                eventArgs.Cancel = true;
            };

            pressFlow.Start();

            Console.WriteLine("Druk op Ctrl+C om te stoppen...");

            // Houd main thread levend
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
