using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService.MultiThread
{
    public partial class MultiThreadService : ServiceBase
    {
        private Thread[] _workerThreads;
        private readonly IServiceThread[] _serviceThreads;
        readonly int _numberOfThreads;
        public MultiThreadService(IServiceThread[] serviceThreads)
        {
            _serviceThreads = serviceThreads;
            _numberOfThreads = serviceThreads.Length;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _workerThreads = new Thread[_numberOfThreads];

           for (var i = 0; i < _numberOfThreads; i++)
            {
                _serviceThreads[i].ServiceStarted = true;
                _workerThreads[i] = new Thread(() => _serviceThreads[i].DoWork());
                _workerThreads[i].Start();
            }
        }

        protected override void OnStop()
        {
            for (int i = 0; i < _numberOfThreads; i++)
            {
                _serviceThreads[i].ServiceStarted = false;
                _workerThreads[i].Join(500);
            }
        }
    }
}
