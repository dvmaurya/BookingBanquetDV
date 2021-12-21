namespace CommonServices.Model.DbModel
{
    public class CustomerPhotoDetail
    {
        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string FileType { get; set; }
        public ulong Size { get; set; }
        public string Date { get; set; }
        public string EmailId { get; set; }
        public int BookingId { get; set; }
    }
}
