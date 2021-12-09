namespace CommonServices.Model.DbModel
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string BookingDate { get; set; }
        public string BookedBy { get; set; }
        public string TotalRooms { get; set; }
        public string TotalFlours { get; set; }
        public string TotalAmount { get; set; }
        public string Advanced { get; set; }
        public string Balance { get; set; }
        public string Confirmation { get; set; }
        public string CreationDate { get; set; }
        public string PhotoName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string UserId { get; set; }
        public bool IsEmailSent { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
