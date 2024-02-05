using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
   
    public class NotificationRequest
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public int CarId { get; set; } // Assuming you want to associate the notification with a car

        // You can add additional properties as needed, such as timestamps, etc.
    }

}
