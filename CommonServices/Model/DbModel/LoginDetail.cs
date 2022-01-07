namespace CommonServices.Model.DbModel
{
    public class LoginDetail
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public bool IsRememberChecked { get; set; }
        public string NumberOfLogin { get; set; }
        public string LastLogin { get; set; }
    }
}
