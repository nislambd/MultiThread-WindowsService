using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService.MultiThread
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var services = new List<IServiceThread>();

            //add the services here that needs to run in each thread
            services.Add(new FileWatcherService());

            //running as service
            if (!Environment.UserInteractive)
            {
                var servicesToRun = new ServiceBase[]
                {
                    new MultiThreadService(services.ToArray()),
                };
                ServiceBase.Run(servicesToRun);
            }
            //running as console app
            else
            {
                var workerThreads = new Thread[services.Count];

                for (int i = 0; i < services.Count; i++)
                {
                    services[i].ServiceStarted = true;
                    workerThreads[i] = new Thread(() => services[i].DoWork());
                    workerThreads[i].Start();

                    Console.WriteLine($"{services[i].GetType().Name} running in thread {i}");
                }

                Console.WriteLine("Service is running in console mode. Press any key to quit.");
                Console.ReadKey();

                for (int i = 0; i < services.Count; i++)
                {
                    services[i].ServiceStarted = false;
                    workerThreads[i].Join(500);
                }
            }
        }
    }
}
