using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.Interfaces
{
    public interface IWorkFlowSettingParser
    {
        string Path { get; set; }
        Dictionary<Type, List<IWorkFlowState>> Parse();
    }
}
