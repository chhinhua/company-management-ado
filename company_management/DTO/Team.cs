namespace company_management.DTO
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IdLeader { get; set; }
        public byte[] Avatar { get; set; }
     
        public Team() { }

        public Team(string name, string description, int idLeader)
        {
            Name = name;
            Description = description;
            IdLeader = idLeader;
        }
    }
}
