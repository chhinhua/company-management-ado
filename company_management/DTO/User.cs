
namespace company_management.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public byte[] Avatar { get; set; }
        public int IdRole { get; set; }
        public int IdPosition { get; set; }

        public User() { }
        
        public User(string username, string password, string fullName,
           string email, string phoneNumber, string address, int idRole)
        {
            Username = username;
            Password = password;
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            IdRole = idRole;
        }
    }
}
