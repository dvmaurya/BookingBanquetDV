namespace CommonServices.Model.DbModel
{
    public class PhotoDetails
    {
        public int Id { get; set; }
        public string PhotoName { get; set; }
        public string FileType { get; set; }
        public ulong Size { get; set; }
        public string Date { get; set; }
        public string EmailId { get; set; }
        public bool IsPhotoActive { get; set; }
        public bool IsPhotoRemoved { get; set; }
    }
}
