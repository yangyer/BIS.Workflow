using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.Interfaces
{
    public interface IWorkFlowState
    {
        string StateName { get; }
        List<IWorkFlowState> AvailableStates { get; }
    }
}
