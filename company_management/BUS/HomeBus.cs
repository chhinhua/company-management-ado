using System;
using System.Linq;
using company_management.Utilities;

namespace company_management.BUS
{
    public class HomeBus
    {
        private readonly Lazy<ProjectBus> _projectBus;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Lazy<TeamBus> _teamBus;
        private readonly Lazy<CheckinCheckoutBus> _cicoBus;
        private readonly Lazy<LeaveRequestBus> _requestBus;
        private readonly Lazy<SalaryBus> _salaryBus;

        public HomeBus()
        {
            _projectBus = new Lazy<ProjectBus>(() => new ProjectBus());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _teamBus = new Lazy<TeamBus>(() => new TeamBus());
            _cicoBus = new Lazy<CheckinCheckoutBus>(() => new CheckinCheckoutBus());
            _requestBus = new Lazy<LeaveRequestBus>(() => new LeaveRequestBus());
            _salaryBus = new Lazy<SalaryBus>(() => new SalaryBus());
        }

        public HomeStatistics GetHomeStatistics()
        {
            return new HomeStatistics
            {
                Project = _projectBus.Value.GetListProjectByPosition().Count,
                Task = _taskBus.Value.GetListTaskByPosition().Count,
                Team = _teamBus.Value.GetListTeamByPosition().Count,
                Timekeeping = _cicoBus.Value.GetListCheckinCheckoutsByPosition().Count,
                LeaveRequest = _requestBus.Value.GetListRequestByPosition().Count,
                Salary = _salaryBus.Value.GetListSalaryByPosition().Sum(s => s.FinalSalary)
            };
        }
    }
}