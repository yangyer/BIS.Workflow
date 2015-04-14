using BIS.WorkFlow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIS.WorkFlow.Core;

namespace BIS.WorkFlow
{
    public class WorkFlowResult<T> : IWorkFlowResult<T> where T : IWorkFlowEntity
    {
        public string DetailMessage { get; private set; }

        public string FriendlyMessage { get; private set; }

        public WorkFlowResultTypes Result { get; private set; }

        public T ReturnObject { get; private set; }

        public Exception Exception { get; private set; }

        internal WorkFlowResult() { }

        public WorkFlowResult(WorkFlowResultTypes resultType, T returnObject, string friendlyMessage, string detailMessage, Exception exception)
        {
            if(exception != null)
            {
                Exception = exception;
                FriendlyMessage = "An error occured.";
                DetailMessage = exception.ToString();
            }

            Result = resultType;
            ReturnObject = returnObject;
            DetailMessage += (!string.IsNullOrWhiteSpace(detailMessage) ? " " + detailMessage : string.Empty);
            FriendlyMessage += (!string.IsNullOrWhiteSpace(friendlyMessage) ? " " + friendlyMessage : string.Empty);
        }

        public static IWorkFlowResult<T> GetSuccessResult(T returnObject, string friendlyMessage)
        {
            return new WorkFlowResult<T>(WorkFlowResultTypes.Success, returnObject, friendlyMessage, "", null);
        }

        public static IWorkFlowResult<T> GetFailResult(string friendlyMessage)
        {
            return new WorkFlowResult<T>(WorkFlowResultTypes.Fail, default(T), friendlyMessage, "", null);
        }

        public static IWorkFlowResult<T> GetExceptionResult(Exception ex)
        {
            return new WorkFlowResult<T>(WorkFlowResultTypes.Exception, default(T), ex.Message, "", ex);
        }
    }
}
