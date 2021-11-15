using SQLite.Net.Attributes;
namespace CommonServices.Model
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string CreationDate { get; set; }
        public bool IsActivate { get; set; }
        public int PasswordChanged { get; set; }
    }
}
