using System;

namespace company_management.DTO
{
    public class Project
    {
        public int Id { get; set; }
        public int IdCreator { get; set; }
        public int IdAssignee { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Progress { get; set; }
        public int IdTeam { get; set; }
        public decimal Bonus { get; set; }
        
        public Project() { }

        public Project(int idCreator, int idAssignee, string name, string description, 
            DateTime startDate, DateTime endDate, int progress, int idTeam, decimal bonus)
        {
            IdCreator = idCreator;
            IdAssignee = idAssignee;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Progress = progress;
            IdTeam = idTeam;
            Bonus = bonus;
        }

    }
}
