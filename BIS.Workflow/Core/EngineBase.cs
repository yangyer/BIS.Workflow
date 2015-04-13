using BIS.WorkFlow.Interfaces;
using System;
using System.Collections.Generic;

namespace BIS.WorkFlow.Core
{
    public abstract class EngineBase
    {
        public Dictionary<Type, List<IWorkFlowState>> WorkFlowStates { get; set; }

        public virtual bool Authorize(IWorkFlowEntity entity)
        {
            return true;
        }
    }
}
