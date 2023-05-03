using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company_management.DTO
{
    public class Salary
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public decimal BasicSalary { get; set; }
        public double TotalHours { get; set; }
        public double OvertimeHours { get; set; }
        public double LeaveHours { get; set; }
        public decimal Bonus { get; set; }
        public decimal Allowance { get; set; }
        public decimal Tax { get; set; }
        public decimal Insurance { get; set; }
        public decimal FinalSalary { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        
        public Salary() { }
        
        public Salary(int idUser, decimal basicSalary, double totalHours, double overtimeHours, double leaveHours, decimal bonus, decimal allowance, decimal tax, decimal insurance, decimal finalSalary)
        {
            IdUser = idUser;
            BasicSalary = basicSalary;
            TotalHours = totalHours;
            OvertimeHours = overtimeHours;
            LeaveHours = leaveHours;
            Bonus = bonus;
            Allowance = allowance;
            Tax = tax;
            Insurance = insurance;
            FinalSalary = finalSalary;
        }
    }
}
