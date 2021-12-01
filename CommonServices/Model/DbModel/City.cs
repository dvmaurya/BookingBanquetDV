namespace CommonServices.Model.DbModel
{
    public class City
    {
        public long Id { get; set; }
        public long StateId {get;set;}
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
