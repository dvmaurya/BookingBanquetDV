namespace CommonServices.Model
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string BookingDate { get; set; }
        public string BookedBy { get; set; }
        public string NumberOfRooms { get; set; }
        public string NumberOfFlours { get; set; }
        public string TotalAmount { get; set; }
        public string Advanced { get; set; }
        public string Balance { get; set; }
        public string Confirmation { get; set; }
        public string CreationDate { get; set; }
        public string PhotoName { get; set; }
        public string UserId { get; set; }
        public bool CanShowPhoto { get; set; }
    }
}
