using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.Core
{
    public enum WorkFlowDirections
    {
        Forward = 0,
        Backward = 1
    }

    public enum WorkFlowResultTypes
    {
        Success = 0,
        Fail = 1,
        Exception = 2
    }
}
