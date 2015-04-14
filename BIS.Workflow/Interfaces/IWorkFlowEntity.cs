using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.Interfaces
{
    public interface IWorkFlowEntity
    {
        IWorkFlowState CurrentState { get; set; }

        IWorkFlowEntity MoveNext(IWorkFlowState nextState);

        IWorkFlowEntity MovePrevious();
    }
}
