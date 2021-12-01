namespace CommonServices.Model.DbModel
{
    public class State
    {
        public long Id { get; set; }
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Type { get; set; }
    }
}
