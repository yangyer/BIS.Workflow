using BIS.WorkFlow.Core;
using BIS.WorkFlow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow
{
    public class Engine : EngineBase
    {
        public List<Func<IWorkFlowEntity, bool>> AuthorizeFunctions { get; private set; }

        public Engine(IWorkFlowSettingParser parser)
        {
            WorkFlowStates = parser.Parse();
        }

        public void RegisterAuthorization(params Func<IWorkFlowEntity, bool>[] authorizeFuncs)
        {
            if (authorizeFuncs != null && authorizeFuncs.Count() > 0)
            {
                AuthorizeFunctions.AddRange(authorizeFuncs);
            }
        }

        //public IWorkFlowResult<IWorkFlowEntity> Eval(IWorkFlowEntity entity, WorkFlowDirections direction)
        //{
        //    try
        //    {
        //        IWorkFlowResult<IWorkFlowEntity> result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("Entity can not be processed!");

        //        var currentWorkFlowState = WorkFlowStates[entity.GetType()].FirstOrDefault(wfs => wfs.StateName == entity.CurrentStateName);

        //        if (currentWorkFlowState == null)
        //        {
        //            return WorkFlowResult<IWorkFlowEntity>.GetFailResult("No workflow states found.");
        //        }

        //        if (direction == WorkFlowDirections.Forward)
        //        {
        //            var nextEntity = entity.MoveNext();
        //            if (currentWorkFlowState.ForwordStates.Any(fs => fs.StateName == nextEntity.CurrentStateName))
        //            {
        //                if (base.Authorize(nextEntity))
        //                {
        //                    result = WorkFlowResult<IWorkFlowEntity>.GetSuccessResult(nextEntity, "Successfully processed.");
        //                }
        //                else
        //                {
        //                    result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("You are not authorize to perform this action.");
        //                }
        //            }
        //            else
        //            {
        //                result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("Error, base on the current entity state, it can not move next in the workflow.");
        //            }
        //        }
        //        else if (direction == WorkFlowDirections.Backward)
        //        {
        //            var prevEntity = entity.MoveNext();
        //            if (currentWorkFlowState.BackwordStates.Any(bs => bs.StateName == prevEntity.CurrentStateName))
        //            {
        //                if (base.Authorize(prevEntity))
        //                {
        //                    result = WorkFlowResult<IWorkFlowEntity>.GetSuccessResult(prevEntity, "Successfully processed.");
        //                }
        //                else
        //                {
        //                    result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("You are not authorize to perform this action.");
        //                }
        //            }
        //        }
                
        //        return result;
        //    }
        //    catch(Exception ex)
        //    {
        //        return WorkFlowResult<IWorkFlowEntity>.GetExceptionResult(ex);
        //    }
        //}

        public IWorkFlowResult<IWorkFlowEntity> Eval(IWorkFlowEntity entity, IWorkFlowState nextState)
        {
            try
            {
                IWorkFlowResult<IWorkFlowEntity> result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("Entity can not be processed!");

                var currentWorkFlowState = WorkFlowStates[entity.GetType()].FirstOrDefault(wfs => wfs == entity);

                if (currentWorkFlowState == null)
                {
                    return WorkFlowResult<IWorkFlowEntity>.GetFailResult("No workflow states found.");
                }
                
                if (currentWorkFlowState.AvailableStates.Any(fs => fs == nextState))
                {
                    if (base.Authorize(entity))
                    {
                        entity.CurrentState = nextState;
                        result = WorkFlowResult<IWorkFlowEntity>.GetSuccessResult(entity, "Successfully processed.");
                    }
                    else
                    {
                        result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("You are not authorize to perform this action.");
                    }
                }
                else
                {
                    result = WorkFlowResult<IWorkFlowEntity>.GetFailResult("Error, base on the current entity state, it can not move next in the workflow.");
                }

                return result;
            }
            catch (Exception ex)
            {
                return WorkFlowResult<IWorkFlowEntity>.GetExceptionResult(ex);
            }
        }

        public override bool Authorize(IWorkFlowEntity entity)
        {
            if (AuthorizeFunctions == null || AuthorizeFunctions.Count() < 1)
            {
                return base.Authorize(entity);
            }
            else
            {
                return AuthorizeFunctions.All(authFunc => authFunc.Invoke(entity));
            }
        }
    }
}
