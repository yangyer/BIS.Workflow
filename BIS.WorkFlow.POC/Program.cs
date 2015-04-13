using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.POC
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine e = new Engine(new ParkingWorkflowConfigParser());
        }
    }
}
