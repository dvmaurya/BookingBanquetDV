using SQLite.Net.Attributes;

namespace CommonServices.Model
{
    public class LoginDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool IsRememberChecked { get; set; }
        public string NumberOfLogin { get; set; }
        public string LastLogin { get; set; }
    }
}
