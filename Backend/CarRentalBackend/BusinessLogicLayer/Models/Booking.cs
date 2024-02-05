using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int CarId { get; set; }
        public DateTime RentalTime { get; set; }
        public bool IsReturning { get; set; }
        public bool Returned { get; set; }
    }
}
