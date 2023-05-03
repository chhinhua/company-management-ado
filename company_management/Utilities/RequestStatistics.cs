namespace company_management.Utilities
{
    public class RequestStatistics
    {
        public int Pending { get; set; } 
        public int Approved { get; set; } 
        public int Cancelled { get; set; } 
        public int Rejected { get; set; } 
        public int AllCount { get; set; } 
        
        public RequestStatistics() {}
    }
}