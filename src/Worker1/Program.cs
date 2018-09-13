using System;
using System.Runtime.Loader;
using System.Threading;

namespace Worker1
{
    class Program
    {
        static void Main(string[] args)
        {
            var processSignalWaiter = new ProcessSignalWaiter();
            var token = processSignalWaiter.GetToken();
            //loop to do actual work
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("A message from worker1.");
                Thread.Sleep(1000);
            }
            //signal that work is complete
            processSignalWaiter.Complete();
            Console.WriteLine("Worker exited.");
        }
    }
    public class ProcessSignalWaiter
    {
        private static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        private static ManualResetEventSlim ended = new ManualResetEventSlim();
        static ProcessSignalWaiter()
        {
            AssemblyLoadContext.Default.Unloading += ctx =>
            {
                //Console.WriteLine("Process exit is fired.");
                cancelTokenSource.Cancel();
                ended.Wait();
                Thread.Sleep(3000);
                //Console.WriteLine("Process is exiting.");
            };
        }
        public CancellationToken GetToken()
        {
            return cancelTokenSource.Token;
        }
        public void Complete()
        {
            //Console.WriteLine("Process complete is called.");
            ended.Set();
        }
    }
}
