namespace company_management.Utilities
{
    public class SalariesStatistics
    {
        public double TotalHours { get; set; } 
        public decimal TotalAllowance { get; set; } 
        public decimal TotalInsurance { get; set; } 
        public decimal TotalTax { get; set; }
        public decimal TotalFinalSalary { get; set; } 
        
        public SalariesStatistics() {}
    }
}