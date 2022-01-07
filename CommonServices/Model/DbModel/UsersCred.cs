namespace CommonServices.Model.DbModel
{
    public class UsersCred
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string MobilePassword { get; set; }
        public string EmailPassword { get; set; }
        public string CreationDate { get; set; }
        public bool IsAdmin { get; set; }
        public int PasswordChanged { get; set; }
    }
}
