using BIS.WorkFlow.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.Interfaces
{
    public interface IWorkFlowResult
    {
        WorkFlowResultTypes Result { get; }
        string FriendlyMessage { get; }
        string DetailMessage { get; }
        Exception Exception { get; }
    }

    public interface IWorkFlowResult<T> : IWorkFlowResult where T : IWorkFlowEntity
    {
        T ReturnObject { get; }
    }
}
