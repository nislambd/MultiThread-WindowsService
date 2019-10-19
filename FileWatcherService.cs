using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace WindowsService.MultiThread
{
    class FileWatcherService:IServiceThread
    {
        private void ProcessFile(object sender, FileSystemEventArgs e)
        {
           Console.WriteLine($"{e.ChangeType}, {e.Name}");
        }

        public bool ServiceStarted { get; set; }
        public void DoWork()
        {
            if (ServiceStarted)
            {
                var watchPath = ConfigurationManager.AppSettings["WatchPath"];
                var watcher = new FileSystemWatcher(watchPath) { EnableRaisingEvents = true };
                watcher.Filter = "*.*";
                watcher.Created += ProcessFile;
                watcher.Deleted += ProcessFile;
                watcher.Changed += ProcessFile;
            }
            else
            {
                Thread.CurrentThread.Abort();
            }
        }
    }
}
