using System;

namespace company_management.DTO
{
    public class Task
    {
        public int Id { get; set; }
        public int IdCreator { get; set; }
        public int IdAssignee { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int Progress { get; set; }
        public int IdTeam { get; set; }
        public decimal Bonus { get; set; }
        public int IdProject { get; set; }

        public Task() { }

        public Task(int idCreator, int idAssignee, string taskName, string description, 
            DateTime deadline, int progress, int idTeam, decimal bonus, int idProject)
        {
            IdCreator = idCreator;
            IdAssignee = idAssignee;
            TaskName = taskName;
            Description = description;
            Deadline = deadline;
            Progress = progress;
            IdTeam = idTeam;
            Bonus = bonus;
            IdProject = idProject;
        }
    }
}
