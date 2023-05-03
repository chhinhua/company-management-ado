using System;

namespace company_management.DTO
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberDay { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public int IdApprover { get; set; }
        
        public LeaveRequest() { }
    }
}
