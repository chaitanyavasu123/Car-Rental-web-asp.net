using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class BookingRequest
    {
        public string UserEmail { get; set; }
        public int CarId { get; set; }
        public DateTime RentalTime { get; set; }
    }

}
