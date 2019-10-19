using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService.MultiThread
{
    public interface IServiceThread
    {
        bool ServiceStarted { get; set; }
        void DoWork();
    }
}
