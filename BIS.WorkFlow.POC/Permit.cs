using BIS.WorkFlow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIS.WorkFlow.POC
{
    public class Facade
    {
        public Engine WorkkflowEngine {
            get
            {
                return new Engine(new ParkingWorkflowConfigParser());
            }
        }

        //public static void SaveChanges(Permits permit, PermitStatus status, bool ChangeStatus)
        //{
        //    permit.DateAssigned = DateTime.Now;
        //    permit.MoveNext(PermitStatus newStatus);
        //}

        public void SaveChanges(Permits permit, PermitStatus status, bool ChangeStatus)
        {
            permit.DateAssigned = DateTime.Now;

            WorkkflowEngine.Eval(permit, status);
            //permit.MoveNext();
        }
    }

    public class Permits : IWorkFlowEntity
    {
        public int Id { get; set; }

        public string PermitName { get; set; }

        public DateTime DateAssigned { get; set; }

        public PermitStatus CurrentPermitStatus { get; set; }

        public IWorkFlowState CurrentState { get; set; }

        public IWorkFlowEntity MoveNext(IWorkFlowState nextState)
        {
            if((this.CurrentState as PermitStatus).Key == 1)
            {
                if (this.DateAssigned < DateTime.Now.AddDays(-14))
                {
                    this.CurrentState = this.CurrentState.AvailableStates.FirstOrDefault(s => (s as PermitStatus).Key == 7);
                    this.CurrentPermitStatus = this.CurrentState.AvailableStates.FirstOrDefault(s => (s as PermitStatus).Key == 7) as PermitStatus;
                }
                else
                {
                    this.CurrentState = this.CurrentState.AvailableStates.FirstOrDefault(s => (s as PermitStatus).Key == 2);
                    this.CurrentPermitStatus = this.CurrentState.AvailableStates.FirstOrDefault(s => (s as PermitStatus).Key == 2) as PermitStatus;
                }
            }

            return this;
        }

        public IWorkFlowEntity MovePrevious()
        {
            throw new NotImplementedException();
        }
    }

    public static class PermitStatusCollection
    {
        public static List<PermitStatus> PermitStatusList
        {
            get
            {
                var config = new Dictionary<int, List<int>>();
                config.Add(1, new List<int> { 2, 7 });

                var permitStatusList = new List<PermitStatus>()
                {
                    new PermitStatus { Key = 1, Name = "UnAssigned" },
                    new PermitStatus { Key = 2, Name = "Assigned" },
                    new PermitStatus { Key = 3, Name = "Delivered_PickedUp" },
                    new PermitStatus { Key = 4, Name = "Not_Delivered_PickedUp" },
                    new PermitStatus { Key = 5, Name = "Returned" },
                    new PermitStatus { Key = 6, Name = "Expired" },
                    new PermitStatus { Key = 7, Name = "Destroyed" }
                };
                
                foreach(var permitStatus in permitStatusList)
                {
                    var curConfig = config[permitStatus.Key];

                    if(curConfig != null)
                    {
                        permitStatus.LoadStates(permitStatusList.Where(p => curConfig.Contains(p.Key)).ToList<IWorkFlowState>());
                    }
                }

                return permitStatusList;
            }
        }
    }

    public class ParkingWorkflowConfigParser : IWorkFlowSettingParser
    {
        public string Path
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<Type, List<IWorkFlowState>> Parse()
        {
            var config = new Dictionary<int, List<int>>();
            config.Add(1, new List<int> { 2, 7 });

            var permitStatusList = new List<PermitStatus>()
                {
                    new PermitStatus { Key = 1, Name = "UnAssigned" },
                    new PermitStatus { Key = 2, Name = "Assigned" },
                    new PermitStatus { Key = 3, Name = "Delivered_PickedUp" },
                    new PermitStatus { Key = 4, Name = "Not_Delivered_PickedUp" },
                    new PermitStatus { Key = 5, Name = "Returned" },
                    new PermitStatus { Key = 6, Name = "Expired" },
                    new PermitStatus { Key = 7, Name = "Destroyed" }
                };

            foreach (var permitStatus in permitStatusList)
            {
                var curConfig = config[permitStatus.Key];

                if (curConfig != null)
                {
                    permitStatus.LoadStates(permitStatusList.Where(p => curConfig.Contains(p.Key)).ToList<IWorkFlowState>());
                }
            }

            var workFlowStates = new Dictionary<Type, List<IWorkFlowState>>();

            workFlowStates.Add(typeof(Permits), permitStatusList.ToList<IWorkFlowState>());

            return workFlowStates;
        }
    }

    public class PermitStatus : IWorkFlowState
    {
        public List<IWorkFlowState> AvailableStates
        {
            get;
            private set;
        }

        public int Key { get; set; }

        public string Name { get; set; }

        public string StateName
        {
            get
            {
                return this.Name;
            }
        }

        public void LoadStates(List<IWorkFlowState> availableStates)
        {
            AvailableStates = availableStates;
        }
    }
}
