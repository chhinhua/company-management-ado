using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.BUS;
using company_management.DTO;
using company_management.Utilities;

namespace company_management.View.UC
{
    public partial class UcSalary : UserControl
    {
        private readonly Lazy<Utils> _utils;
        private readonly Lazy<SalaryBus> _salaryBus;

        public UcSalary()
        {
            InitializeComponent();
            _utils = new Lazy<Utils>(() => new Utils());
            _salaryBus = new Lazy<SalaryBus>(() => new SalaryBus());
        }

        private void LoadData(List<Salary> salaries)
        {
            LoadDataGridview(salaries);
            LoadSalariesStatistics(salaries);
            _utils.Value.CheckCalculateSalaryStatus(btn_caculateSalary);
            _utils.Value.CheckCalculateSalaryStatus(btnRefresh);
        }

        private void LoadDataGridview(List<Salary> salaries)
        {
            var salaryBus = _salaryBus.Value;
            salaryBus.LoadDataGridview(salaries, datagridview_salary);
        }

        private List<Salary> GetData()
        {
            var salaryBus = _salaryBus.Value;
            return salaryBus.GetListSalaryByPosition();
        }

        private void LoadSalariesStatistics(List<Salary> salaries)
        {
            var salaryBus = _salaryBus.Value;
            var salariesStatistics = salaryBus.GetSalariesStatistics(salaries);

            label_totalHours.Text = salariesStatistics.TotalHours.ToString("#,##0.00'h'");
            label_totalAllowance.Text = salariesStatistics.TotalAllowance.ToString("C");
            label_totalInsurance.Text = salariesStatistics.TotalInsurance.ToString("C");
            label_totalTax.Text = salariesStatistics.TotalTax.ToString("C");
            label_totalFinalSalary.Text = salariesStatistics.TotalFinalSalary.ToString("C");
        }
        
        private void UCSalary_Load(object sender, EventArgs e)
        {
            LoadData(GetData());
        }

        private void btn_caculateSalary_Click(object sender, EventArgs e)
        {
            FormCalculateSalary form = new FormCalculateSalary();
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData(GetData());
        }
    }
}