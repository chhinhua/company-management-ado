﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using company_management.DTO;

namespace company_management.DAO
{
    public class SalaryDao
    {
        private readonly DBConnection _dBConnection;
        private readonly Lazy<CheckinCheckoutDao> _cicoDao;
        private readonly Lazy<TaskDao> _taskDao;
        private readonly Lazy<UserDao> _userDao;

        public SalaryDao()
        {
            _dBConnection = new DBConnection();
            _cicoDao = new Lazy<CheckinCheckoutDao>(() => new CheckinCheckoutDao());
            _taskDao = new Lazy<TaskDao>(() => new TaskDao());
            _userDao = new Lazy<UserDao>(() => new UserDao());
        }

        public List<Salary> GetAllSalary()
        {
            string query = string.Format("SELECT * FROM salary");
            return _dBConnection.GetListObjectsByQuery<Salary>(query);
        }

        public List<Salary> GetMySalary(int idUser)
        {
            return GetAllSalary().Where(s => s.IdUser == idUser).ToList();
        }

        private Salary SetBasicInfoOfSalary(int idUser)
        {
            Salary salary = new Salary();
    
            using (SqlConnection connection = new SqlConnection(_dBConnection.connString))
            {
                string query = "SELECT basicSalary, allowance, insurance, tax FROM user_salary WHERE idUser = @idUser";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUser", idUser);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal basicSalary = Convert.ToDecimal(reader["basicSalary"]);
                            decimal allowance = Convert.ToDecimal(reader["allowance"]);
                            decimal insurance = Convert.ToDecimal(reader["insurance"]);
                            decimal tax = Convert.ToDecimal(reader["tax"]);
                            
                            salary.IdUser = idUser;
                            salary.BasicSalary = basicSalary;
                            salary.Allowance = allowance;
                            salary.Insurance = insurance;
                            salary.Tax = tax;
                        }
                    }
                }
            }

            return salary;
        }
        
        private Salary CalculateSalaryForEmployee(int idUser, DateTime fromDate, DateTime toDate)
        {
            Salary salary = SetBasicInfoOfSalary(idUser);

            using (SqlConnection connection = new SqlConnection(_dBConnection.connString))
            {
                connection.Open();

                // Tính toán các thành phần của lương
                double totalHours = 0;
                double overtimeHours = 0;
                double leaveHours = 0;
                decimal bonus = 0;

                using (var taskDao = _taskDao.Value)
                {
                    bonus = taskDao.CalculateBonusForEmployee(idUser, fromDate, toDate);
                }

                using (var cicoDao = _cicoDao.Value)
                {
                    totalHours = cicoDao.GetTotalHours(idUser, fromDate, toDate, connection);
                    overtimeHours = cicoDao.GetOvertimeHours(idUser, fromDate, toDate, connection);
                    leaveHours = cicoDao.GetLeaveHours(idUser, fromDate, toDate, connection);
                }

                // Tính toán lương
                decimal hourlyRate = salary.BasicSalary;
                
                decimal hourlyPay = ((decimal)(totalHours - overtimeHours) * hourlyRate);
                decimal overtimePay = (decimal)overtimeHours * 1.5m * hourlyRate;
                decimal leavePay = (decimal)leaveHours * hourlyRate;
                decimal finalSalary = hourlyPay + overtimePay + bonus + salary.Allowance - leavePay - salary.Tax - salary.Insurance;
                
                salary.TotalHours = totalHours;
                salary.OvertimeHours = overtimeHours;
                salary.LeaveHours = leaveHours;
                salary.FinalSalary = finalSalary;
            }

            return salary;
        }

        private List<int> GetUserIdList()
        {
            using (var userDao = _userDao.Value)
            {
                List<User> userList = userDao.GetAllUser();
                List<int> userIds = userList.Select(u => u.Id).ToList();
                return userIds;
            }
        }

        public void CalculateAndSaveSalaryForAllEmployees(DateTime fromDate, DateTime toDate)
        {
            List<int> userIds = GetUserIdList();

            using (SqlConnection connection = new SqlConnection(_dBConnection.connString))
            {
                connection.Open();
                foreach (int idUser in userIds)
                {
                    Salary salary = CalculateSalaryForEmployee(idUser, fromDate, toDate);

                    // Lưu thông tin lương vào bảng salary
                    string query =
                        "INSERT INTO salary (idUser, basicSalary, totalHours, overtimeHours, leaveHours, bonus, allowance, tax, insurance, finalSalary) " +
                        "VALUES (@idUser, @basicSalary, @totalHours, @overtimeHours, @leaveHours, @bonus, @allowance, @tax, @insurance, @finalSalary)";
                    SqlCommand insertCommand = new SqlCommand(query, connection);
                    insertCommand.Parameters.AddWithValue("@idUser", salary.IdUser);
                    insertCommand.Parameters.AddWithValue("@basicSalary", salary.BasicSalary);
                    insertCommand.Parameters.AddWithValue("@totalHours", (decimal)salary.TotalHours);
                    insertCommand.Parameters.AddWithValue("@overtimeHours", (decimal)salary.OvertimeHours);
                    insertCommand.Parameters.AddWithValue("@leaveHours", (decimal)salary.LeaveHours);
                    insertCommand.Parameters.AddWithValue("@bonus", salary.Bonus);
                    insertCommand.Parameters.AddWithValue("@allowance", salary.Allowance);
                    insertCommand.Parameters.AddWithValue("@tax", salary.Tax);
                    insertCommand.Parameters.AddWithValue("@insurance", salary.Insurance);
                    insertCommand.Parameters.AddWithValue("@finalSalary", salary.FinalSalary);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public decimal CalculateBonus(double kpiValue, double averageProgress)
        {
            decimal bonus = 0;
            double kpiWithProgress = kpiValue + averageProgress;

            // Tính toán lương bonus
            if (kpiWithProgress >= 0.7)
            {
                bonus = (decimal)kpiWithProgress * (decimal)Constants.DEFAULT_BASIC_SALARY;
            }

            return bonus;
        }
    }
}