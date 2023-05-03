using System;
using System.Windows.Forms;
using company_management.BUS;

namespace company_management.View.UC
{
    public partial class UcHome : UserControl
    {
        private readonly HomeBus _homeBus;
        
        public UcHome()
        {
            _homeBus = new HomeBus();
            InitializeComponent();
        }

        private void LoadData()
        {
            LoadHomeStatistics();
        }

        private void LoadHomeStatistics()
        {
            var homeStatistics = _homeBus.GetHomeStatistics();
            label_project.Text = homeStatistics.Project.ToString();
            label_task.Text = homeStatistics.Task.ToString();
            label_team.Text = homeStatistics.Team.ToString();
            label_timekeeping.Text = homeStatistics.Timekeeping.ToString();
            label_request.Text = homeStatistics.LeaveRequest.ToString();
            label_salary.Text = homeStatistics.Salary.ToString("C");
        }

        private void UcHome_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
