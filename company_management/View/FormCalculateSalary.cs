using System;
using System.Windows.Forms;
using company_management.DAO;

namespace company_management.View
{
    public partial class FormCalculateSalary : Form
    {
        private readonly Lazy<SalaryDao> _salaryDao;

        public FormCalculateSalary()
        {
            InitializeComponent();
            _salaryDao = new Lazy<SalaryDao>(() => new SalaryDao());
        }

        private void Button_calculateByMonth_Click(object sender, EventArgs e)
        {
            int month = DateTimePicker_calculateByMonth.Value.Month;
            int year = DateTimePicker_calculateByMonth.Value.Year;

            // Lấy ngày đầu tiên của tháng đó
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Lấy ngày cuối cùng của tháng đó
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Tính lương cho tháng đó
            var salaryDao = _salaryDao.Value;
            salaryDao.CalculateAndSaveSalaryForAllEmployees(firstDayOfMonth, lastDayOfMonth);
        }

        private void button_calculateByDay_Click(object sender, EventArgs e)
        {
            var salaryDao = _salaryDao.Value;
            DateTime from = DateTimePicker_fromDate.Value;
            DateTime to = DateTimePicker_endDate.Value;
            salaryDao.CalculateAndSaveSalaryForAllEmployees(from, to);
        }
    }
}