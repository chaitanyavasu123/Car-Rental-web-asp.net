using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Maker { get; set; }
        public string Model { get; set; }
        public decimal RentalPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
