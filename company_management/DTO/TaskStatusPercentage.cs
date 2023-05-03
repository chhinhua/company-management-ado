
namespace company_management.DTO
{
    public class TaskStatusPercentage
    {
        public double TodoPercent { get; set; }
        public double InprogressPercent { get; set; }
        public double DonePercent { get; set; }

        public TaskStatusPercentage(){}
        
        public TaskStatusPercentage(double todoPercent, double inprogressPercent, double donePercent)
        {
            TodoPercent = todoPercent;
            InprogressPercent = inprogressPercent;
            DonePercent = donePercent;
        }
    }
}
