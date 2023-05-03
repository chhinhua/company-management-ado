using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;

namespace company_management.BUS
{
    public class SalaryBus
    {
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<SalaryDao> _salaryDao;
        private readonly Lazy<List<Salary>> _listSalary;

        public SalaryBus()
        {
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _salaryDao = new Lazy<SalaryDao>(() => new SalaryDao());
            _listSalary = new Lazy<List<Salary>>(() => new List<Salary>());
        }
        
        public void LoadDataGridview(List<Salary> listSalary, DataGridView dataGridView)
        {
            var userDao = _userDao.Value;

            dataGridView.ColumnCount = 13;
            dataGridView.Columns[0].Name = "Mã";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Họ tên";
            dataGridView.Columns[2].Name = "Lương cơ bản";
            dataGridView.Columns[3].Name = "Tổng giờ làm";
            dataGridView.Columns[4].Name = "Tăng ca";
            dataGridView.Columns[5].Name = "Nghỉ";
            dataGridView.Columns[6].Name = "Thưởng";
            dataGridView.Columns[7].Name = "Phụ cấp";
            dataGridView.Columns[8].Name = "Bảo hiểm";
            dataGridView.Columns[9].Name = "TTNCN";
            dataGridView.Columns[10].Name = "Thực nhận";
            dataGridView.Columns[11].Name = "Từ ngày";
            dataGridView.Columns[12].Name = "Đến ngày";
            for (int i = 1; i < 13; i++) { dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
            for (int i = 6; i < 11; i++) { dataGridView.Columns[i].DefaultCellStyle.Format = "C"; }
            dataGridView.Rows.Clear();

            foreach (var s in listSalary)
            {
                string fullName = userDao.GetUserById(s.IdUser).FullName;
                dataGridView.Rows.Add(s.Id, fullName, s.BasicSalary.ToString("$0.00/h"), 
                    s.TotalHours.ToString("0.0'h'"), s.OvertimeHours.ToString("0.0'h'"), s.LeaveHours.ToString("0.0'h'"), s.Bonus, s.Allowance, 
                    s.Tax, s.Insurance , s.FinalSalary, s.FromDate.ToString("d-M-yyyy"), s.ToDate.ToString("d/M/yyyy"));
            }
        }

        public List<Salary> GetListSalaryByPosition()
        {
            var salaries = _listSalary.Value;
            ClearListSalary(salaries);
            
            var salaryDao = _salaryDao.Value;
            var userBus = _userBus.Value;
            
            if (userBus.IsManager() || userBus.IsHumanResources())
            {salaries = salaryDao.GetAllSalary();}
            else
            {salaries = salaryDao.GetMySalary(UserSession.LoggedInUser.Id);}

            return salaries;
        }

        private void ClearListSalary(List<Salary> listSalary)
        {
            listSalary.Clear();
            listSalary.TrimExcess();
        }
        
        public SalariesStatistics GetSalariesStatistics(List<Salary> salaries)
        {
            return new SalariesStatistics
            {
                TotalHours = salaries.Sum(s => s.TotalHours),
                TotalAllowance = salaries.Sum(s=>s.Allowance),
                TotalInsurance = salaries.Sum(s=>s.Insurance),
                TotalTax = salaries.Sum(s=>s.Tax),
                TotalFinalSalary = salaries.Sum(s=>s.FinalSalary)
            };
        }
    }
}