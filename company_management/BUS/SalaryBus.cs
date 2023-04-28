﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using company_management.DAO;
using company_management.DTO;
using company_management.Utilities;
using company_management.View.UC;

namespace company_management.BUS
{
    public class SalaryBus
    {
        private readonly Lazy<Utils> _utils;
        private readonly Lazy<TeamDao> _teamDao;
        private readonly Lazy<UserDao> _userDao;
        private readonly Lazy<UserBus> _userBus;
        private readonly Lazy<TaskBus> _taskBus;
        private readonly Lazy<SalaryDao> _salaryDao;
        private readonly Lazy<List<Salary>> _listSalary;

        public SalaryBus()
        {
            _utils = new Lazy<Utils>(() => new Utils());
            _teamDao = new Lazy<TeamDao>(() => new TeamDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
            _userBus = new Lazy<UserBus>(() => new UserBus());
            _taskBus = new Lazy<TaskBus>(() => new TaskBus());
            _salaryDao = new Lazy<SalaryDao>(() => new SalaryDao());
            _listSalary = new Lazy<List<Salary>>(() => new List<Salary>());
        }
        
        public void LoadDataGridview(List<Salary> listSalary, DataGridView dataGridView)
        {
            var userDao = _userDao.Value;

            dataGridView.ColumnCount = 11;
            dataGridView.Columns[0].Name = "Mã";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Họ tên";
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns[2].Name = "Lương cơ bản ($/h)";
            dataGridView.Columns[3].Name = "Tổng giờ làm (h)";
            dataGridView.Columns[4].Name = "Tăng ca (h)";
            dataGridView.Columns[5].Name = "Giờ nghỉ (h)";
            dataGridView.Columns[6].Name = "Thưởng ($)";
            dataGridView.Columns[7].Name = "Phụ cấp ($)";
            dataGridView.Columns[8].Name = "Bảo hiểm ($)";
            dataGridView.Columns[9].Name = "TTNCN ($)";
            dataGridView.Columns[10].Name = "Thực nhận ($)";
            dataGridView.Rows.Clear();

            foreach (var s in listSalary)
            {
                string fullName = userDao.GetUserById(s.IdUser).FullName;
                dataGridView.Rows.Add(s.Id, fullName, s.BasicSalary, s.TotalHours, s.OvertimeHours, 
                                    s.LeaveHours, s.Bonus, s.Allowance, s.Tax, s.Insurance , s.FinalSalary);
            }
        }

        public List<Salary> GetListSalaryByPosition()
        {
            var salaries = _listSalary.Value;
            var salaryDao = _salaryDao.Value;
            var userBus = _userBus.Value;
            ClearListSalary(salaries);

            string position = userBus.GetUserPosition();
            salaries = position.Equals("Manager") ? salaryDao.GetAllSalary() : salaryDao.GetMySalary(UserSession.LoggedInUser.Id);
           
            return salaries;
        }

        private void ClearListSalary(List<Salary> listSalary)
        {
            listSalary.Clear();
            listSalary.TrimExcess();
        }
    }
}