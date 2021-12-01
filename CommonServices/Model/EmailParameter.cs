namespace CommonServices.Model
{
    public class EmailParameter
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public BookingDetails BookingDetails { get; set; }
        public Users Users { get; set; }
        public string Otp { get; set; }
        public bool IsSignup { get; set; }
        public bool IsRecovery { get; set; }
    }
}
