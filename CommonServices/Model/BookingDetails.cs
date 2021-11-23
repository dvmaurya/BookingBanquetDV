using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Model
{
    public class BookingDetails
    {
        public string CustomerName { get; set; }
        public string BookingDate { get; set; }
        public string BookedBy { get; set; }
        public string NumberOfRooms { get; set; }
        public string NumberOfFlours { get; set; }
        public string TotalAmount { get; set; }
        public string Advanced { get; set; }
        public string Balance { get; set; }
        public string Confirmation { get; set; }
    }
}
